using Brick_Breaker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Brick_Breaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameState gameState;
        private List<Rectangle> canvasBricks = new List<Rectangle>();
        private GameLevelFactory gameLevelFactory = new GameLevelFactory();
        Stopwatch stopWatch = new Stopwatch();
        MediaPlayer mediaPlayer = new MediaPlayer();


        public MainWindow()
        {
            gameState = new GameState(gameLevelFactory.Levels[0]);
            
            Height = GameSettings.ScreenHeight + 100;
            Width = GameSettings.ScreenWidth + 300;

            InitializeComponent();

            SetupGameCanvas();
        }
        private void SetupGameCanvas()
        {
            Screen.Height = GameSettings.ScreenHeight;
            Screen.Width = GameSettings.ScreenWidth;

            foreach (Brick brick in gameState.Level.Bricks)
            {
                Rectangle rectangle = new Rectangle { Width = GameSettings.BaseWidth, Height = GameSettings.BaseHeight };
                rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(brick.ImageSource, UriKind.Absolute)));
                Canvas.SetBottom(rectangle, brick.Row * GameSettings.BaseHeight+GameSettings.PaddleHeight);
                Canvas.SetLeft(rectangle, brick.Col * GameSettings.BaseWidth);
                Screen.Children.Add(rectangle);
                canvasBricks.Add(rectangle);
            }
        }

        private void Draw()
        {
            DrawPaddle();
            DrawBall();
            DrawBricks();
            DrawItems();

            levelText.Text = gameState.Level.ID.ToString();
            livesText.Text = gameState.Lives.ToString();
            if (stopWatch.ElapsedMilliseconds % 10 == 0)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds);
                timeText.Text = ts.ToString(@"mm\:ss");
            }
            scoreText.Text = gameState.Score.ToString();

        }
        private async Task GameLoop()
        {
            stopWatch.Start();

            int counter = 0;
            while (!gameState.IsGameOver && !gameState.IsLevelOver)
            {
                if (counter % 5 == 0) await Task.Delay(1);
                counter++;

                if (!gameState.Ball.IsGlued )
                {
                    gameState.Ball.Move();
                    if(gameState.BallXWall())
                    {
                        mediaPlayer.Open(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/Bounce.wav", UriKind.Absolute));
                        mediaPlayer.Play();
                    }
                    if(gameState.BallXPaddle())
                    {
                        mediaPlayer.Open(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/Bounce.wav", UriKind.Absolute));
                        mediaPlayer.Play();
                    }
                    gameState.CheckFireBall();
                    if(gameState.BallXBricks())
                    {
                        mediaPlayer.Open(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/Break.wav", UriKind.Absolute));
                        mediaPlayer.Play();
                    }

                    if (gameState.Ball.OutOfBounds)
                    {
                        gameState.RestartBall();
                    }
                }
                else
                    gameState.Ball.MoveTo(gameState.Paddle.Location);
                
                if(gameState.ItemXPaddle())
                {
                    mediaPlayer.Open(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/DropItem.wav", UriKind.Absolute));
                    mediaPlayer.Play();
                }
                gameState.ItemXFloor();
                gameState.MoveAllItems();

                if (gameState.PressedKey == Key.Right) gameState.Paddle.Move(1);
                if (gameState.PressedKey == Key.Left) gameState.Paddle.Move(-1);
                Draw();
            }
            stopWatch.Stop();

            if (gameState.IsLevelOver)
            {
                TimeSpan ts = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds);
                levelTime.Text = "Time:" + ts.ToString(@"mm\:ss");
                levelScore.Text = "Score: " + gameState.Score.ToString();
                LevelOverMenu.Visibility = Visibility.Visible;
            }
            else
            {
                mediaPlayer.Open(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/Gameover.wav", UriKind.Absolute));
                mediaPlayer.Play();
                GameOverMenu.Visibility = Visibility.Visible;
                finalScore.Text = "Score: " + gameState.Score.ToString();
            }
        }
        public void DrawPaddle()
        {
            Paddle.Height = GameSettings.PaddleHeight;
            Paddle.Width = gameState.Paddle.Width;
            Paddle.Fill = new ImageBrush(new BitmapImage(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/Paddle.jpg", UriKind.Absolute)));
            Canvas.SetLeft(Paddle, gameState.Paddle.Location - gameState.Paddle.Width / 2);
            Canvas.SetBottom(Paddle, 0);
        }
        public void DrawBall()
        {
            Ball.Height = gameState.Ball.Radius * 2;
            Ball.Width = gameState.Ball.Radius * 2;
            if(gameState.Ball.Type==Models.Ball.BallType.Regular) Ball.Fill = new ImageBrush(new BitmapImage(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/MetalBall.png", UriKind.Absolute)));
            else Ball.Fill = new ImageBrush(new BitmapImage(new Uri("C:/Users/Vitaly/source/repos/Brick Breaker/Brick Breaker/Resources/FireBall.png", UriKind.Absolute)));
            Canvas.SetLeft(Ball, gameState.Ball.X - gameState.Ball.Radius);
            Canvas.SetBottom(Ball, gameState.Ball.Y - gameState.Ball.Radius);
        }

        public void DrawBricks()
        {
            for(int i=0; i<canvasBricks.Count; i++)
            {
                if (gameState.Level.Bricks[i].IsCracked) canvasBricks[i].Fill = new ImageBrush(new BitmapImage(new Uri(gameState.Level.Bricks[i].CrackedImageSource, UriKind.Absolute)));
                if (gameState.Level.Bricks[i].IsBroken) canvasBricks[i].Visibility = Visibility.Hidden;
                else canvasBricks[i].Visibility = Visibility.Visible;
            }
        }
        public void DrawItems()
        {
            for (int i = Screen.Children.Count - 1; ; i--)
            {
                if (Screen.Children.Cast<FrameworkElement>().Any(x => x.Tag != null && x.Tag.ToString() == "DI"))
                    Screen.Children.RemoveAt(i);
                else break;
            }
            foreach(DropItem dropItem in gameState.DropItems)
            {
                Rectangle rectangle = new Rectangle { Width = GameSettings.BaseHeight, Height = GameSettings.BaseHeight };
                rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(dropItem.ImageSource, UriKind.Absolute)));
                rectangle.Tag = "DI";
                Canvas.SetBottom(rectangle, dropItem.Y - 15);
                Canvas.SetLeft(rectangle, dropItem.X - 15);
                Screen.Children.Add(rectangle);
            }
        }
        private async void PlayAgainClick(object sender, RoutedEventArgs e)
        {
            gameState = new GameState(gameLevelFactory.Levels[0]);
            foreach (Brick brick in gameState.Level.Bricks) brick.IsBroken = false;
            GameOverMenu.Visibility = Visibility.Hidden;
            stopWatch.Reset();
            await GameLoop();
        }
        private async void PlayNextClick(object sender, RoutedEventArgs e)
        {
            int level = gameState.Level.ID;

            if (level + 1 <= gameLevelFactory.Levels.Count)
            {
                Screen.Children.RemoveRange(2, canvasBricks.Count);
                canvasBricks.Clear();

                gameState.StartLevel(gameLevelFactory.Levels[level]);
                SetupGameCanvas();

                LevelOverMenu.Visibility = Visibility.Hidden;
                stopWatch.Reset();
                await GameLoop();
            }
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            gameState.KeyPressed(e.Key);
            if (e.Key == Key.N) gameState.SkipLevel();
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            gameState.KeyReleased();

        }
        private async void Screen_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }
    }
}
