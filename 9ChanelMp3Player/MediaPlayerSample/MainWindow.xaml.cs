using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MediaPlayerSample
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        private MediaPlayer sampleMediaPlayer = new MediaPlayer();
        private MediaPlayer[] _players = new MediaPlayer[9];
        public delegate void timeTick();
        DispatcherTimer[] ticks = new DispatcherTimer[9];
        string filePath;
        private MusicControl[] _controls = new MusicControl[9];

        class MusicControl
        {
            public Button PlayButton = new Button();
            public Button PauseButton = new Button();
            public Button StopButton = new Button();
            public Button LoadButton = new Button();

            public Label VolumeLabel = new Label();
            public Label ProgressLabel = new Label();
            public Label SeperatorLabel = new Label();
            public Label TotalTimeLabel = new Label();
            public Label FileNameLabel = new Label();

            public Slider VolumeSlider = new Slider();
            public Slider ProgressSlider = new Slider();

            public MusicControl(int index, Canvas canvas)
            {
                int y = index * 200;
                int x = 0;
                if(index > 4)
                {
                    x = 550;
                    y = (index - 5) * 200;
                }

                canvas.Children.Add(PlayButton);
                canvas.Children.Add(PauseButton);
                canvas.Children.Add(StopButton);
                canvas.Children.Add(LoadButton);
                canvas.Children.Add(VolumeLabel);
                canvas.Children.Add(ProgressLabel);
                canvas.Children.Add(SeperatorLabel);
                canvas.Children.Add(TotalTimeLabel);
                canvas.Children.Add(FileNameLabel);
                canvas.Children.Add(VolumeSlider);
                canvas.Children.Add(ProgressSlider);

                PlayButton.Content = "재생";
                PlayButton.HorizontalAlignment = HorizontalAlignment.Left;
                PlayButton.Height = 27;
                PlayButton.Margin = new Thickness(39 + x, 187 + y, 0, 0);
                PlayButton.VerticalAlignment = VerticalAlignment.Top;
                PlayButton.Width = 72;

                PauseButton.Content = "멈춤";
                PauseButton.HorizontalAlignment = HorizontalAlignment.Left;
                PauseButton.Height = 27;
                PauseButton.Margin = new Thickness(116 + x, 187 + y, 0, 0);
                PauseButton.VerticalAlignment = VerticalAlignment.Top;
                PauseButton.Width = 72;

                StopButton.Content = "중지";
                StopButton.HorizontalAlignment = HorizontalAlignment.Left;
                StopButton.Height = 27;
                StopButton.Margin = new Thickness(193 + x, 187 + y, 0, 0);
                StopButton.VerticalAlignment = VerticalAlignment.Top;
                StopButton.Width = 72;

                LoadButton.Content = "불러오기";
                LoadButton.HorizontalAlignment = HorizontalAlignment.Left;
                LoadButton.Height = 27;
                LoadButton.Margin = new Thickness(279 + x, 187 + y, 0, 0);
                LoadButton.VerticalAlignment = VerticalAlignment.Top;
                LoadButton.Width = 72;

                VolumeLabel.Content = "음량 : 0";
                VolumeLabel.HorizontalAlignment = HorizontalAlignment.Left;
                VolumeLabel.Height = 26;
                VolumeLabel.Margin = new Thickness(464 + x, 190 + y, 0, 0);
                VolumeLabel.VerticalAlignment = VerticalAlignment.Top;
                VolumeLabel.Width = 96;

                ProgressLabel.Content = "00:00";
                ProgressLabel.HorizontalAlignment = HorizontalAlignment.Left;
                ProgressLabel.Height = 26;
                ProgressLabel.Margin = new Thickness(464 + x, 150 + y, 0, 0);
                ProgressLabel.VerticalAlignment = VerticalAlignment.Top;
                ProgressLabel.Width = 39;

                SeperatorLabel.Content = "/";
                SeperatorLabel.HorizontalAlignment = HorizontalAlignment.Left;
                SeperatorLabel.Height = 26;
                SeperatorLabel.Margin = new Thickness(502 + x, 150 + y, 0, 0);
                SeperatorLabel.VerticalAlignment = VerticalAlignment.Top;
                SeperatorLabel.Width = 22;

                TotalTimeLabel.Content = "00:00";
                TotalTimeLabel.HorizontalAlignment = HorizontalAlignment.Left;
                TotalTimeLabel.Height = 26;
                TotalTimeLabel.Margin = new Thickness(519 + x, 150 + y, 0, 0);
                TotalTimeLabel.VerticalAlignment = VerticalAlignment.Top;
                TotalTimeLabel.Width = 39;

                FileNameLabel.Content = "파일명";
                FileNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
                FileNameLabel.Height = 26;
                FileNameLabel.Margin = new Thickness(39 + x, 82 + y, 0, 0);
                FileNameLabel.VerticalAlignment = VerticalAlignment.Top;
                FileNameLabel.Width = 521;

                VolumeSlider.HorizontalAlignment = HorizontalAlignment.Left;
                VolumeSlider.Height = 22;
                VolumeSlider.Margin = new Thickness(372 + x, 192 + y, 0, 0);
                VolumeSlider.VerticalAlignment = VerticalAlignment.Top;
                VolumeSlider.Width = 79;

                ProgressSlider.HorizontalAlignment = HorizontalAlignment.Left;
                ProgressSlider.Height = 20;
                ProgressSlider.Margin = new Thickness(39 + x, 152 + y, 0, 0);
                ProgressSlider.VerticalAlignment = VerticalAlignment.Top;
                ProgressSlider.Width = 412;

                PlayButton.Tag = index;
                PauseButton.Tag = index;
                StopButton.Tag = index;
                LoadButton.Tag = index;
                VolumeLabel.Tag = index;
                ProgressLabel.Tag = index;
                SeperatorLabel.Tag = index;
                TotalTimeLabel.Tag = index;
                FileNameLabel.Tag = index;
                VolumeSlider.Tag = index;
                ProgressSlider.Tag = index;

                VolumeSlider.Maximum = 100.0;
                VolumeSlider.Minimum = 0.0;
                VolumeSlider.Value = 50.0;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            //if (filePath == null)
            //{
            //    artistNameLabel.Content = "Please Load an .mp3 File.";
            //}
            for(int i = 0; i < _controls.Length; i++)
            {
                MusicControl control = new MusicControl(i, canvas1);
                _controls[i] = control;
                SetEvents(control);
            }
            InitMediaPlayers();
        }

        private void SetEvents(MusicControl control)
        {
            control.PlayButton.Click += playButton_Click;
            control.PauseButton.Click += pauseButton_Click;
            control.StopButton.Click += stopButton_Click;
            control.LoadButton.Click += loadButton_Click;

            control.VolumeSlider.ValueChanged += volumeSlider_ValueChanged;
            control.ProgressSlider.ValueChanged += progressSlider_ValueChanged;

            control.ProgressSlider.AddHandler(Thumb.DragDeltaEvent, new DragDeltaEventHandler((sender, e) => 
            {
                Slider slider = (Slider)sender;
                int index = (int)slider.Tag;
                _players[index].Position = new TimeSpan(0, 0, 0, 0, (int)slider.Value);

                TimeSpan newTimeSpan = _players[index].Position;
                _controls[index].ProgressLabel.Content = newTimeSpan.ToString(@"mm\:ss");
            }));
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            //if (filePath == null)
            //{
            //    artistNameLabel.Content = "Please Load an .mp3 File.";
            //}
            Button btn = sender as Button;
            _players[(int)btn.Tag].Play();
            //sampleMediaPlayer.Play();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            //sampleMediaPlayer.Pause();
            Button btn = sender as Button;
            _players[(int)btn.Tag].Pause();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            //sampleMediaPlayer.Stop();
            Button btn = sender as Button;
            _players[(int)btn.Tag].Stop();
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            slider.Maximum = 100.0;
            slider.Minimum = 0;
            int index = (int)slider.Tag;

            _players[index].Volume = slider.Value / 100.0;
            _controls[index].VolumeLabel.Content = "음량 : " + (int)slider.Value;
        }

        private void loadedMusic(object sender, EventArgs e)
        {

            MediaPlayer player = (MediaPlayer)sender;
            int index = Array.IndexOf(_players, player);
            _controls[index].ProgressSlider.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
            _controls[index].TotalTimeLabel.Content = player.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            ticks[index] = new DispatcherTimer();
            ticks[index].Tag = index;
            ticks[index].Interval = TimeSpan.FromMilliseconds(1);
            ticks[index].Tick += ticks_Tick;
            ticks[index].Start();
        }

        void ticks_Tick(object sender, object e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            int index = (int)timer.Tag;

            TimeSpan newTimeSpan = _players[index].Position;
            _controls[index].ProgressSlider.Value = newTimeSpan.TotalMilliseconds;
            _controls[index].ProgressLabel.Content = newTimeSpan.ToString("mm':'ss':'ff");
        }

        private void progressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;
            int index = (int)slider.Tag;
            TimeSpan newTimeSpan = _players[index].Position;




            _controls[index].ProgressLabel.Content = newTimeSpan.ToString(@"mm\:ss");

        }

        private void progressSlider_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Slider slider = (Slider)sender;
            int index = (int)slider.Tag;
            _players[index].Position = new TimeSpan(0, 0, 0, 0, (int)slider.Value);

            TimeSpan newTimeSpan = _players[index].Position;
            _controls[index].ProgressLabel.Content = newTimeSpan.ToString(@"mm\:ss");
        }

        private void progressSlider_DragEnd(object sender, DragEventArgs e)
        {
            Slider slider = (Slider)sender;
            int index = (int)slider.Tag;
            _players[index].Position = new TimeSpan(0, 0, 0, 0, (int)slider.Value);

            TimeSpan newTimeSpan = _players[index].Position;
            _controls[index].ProgressLabel.Content = newTimeSpan.ToString(@"mm\:ss");
        }

        class MusicID3Tag

        {
            public byte[] TAGID = new byte[3];      //  3
            public byte[] Title = new byte[30];     //  30
            public byte[] Artist = new byte[30];    //  30 
            public byte[] Album = new byte[30];     //  30 
            public byte[] Year = new byte[4];       //  4 
            public byte[] Comment = new byte[30];   //  30 
            public byte[] Genre = new byte[1];      //  1

        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = (int)btn.Tag;

            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".mp3"; 
            openFileDlg.Filter = "MP3 file (.mp3)|*.mp3"; 

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                filePath = openFileDlg.FileName;

                _players[index].Open(new Uri(filePath));
                _players[index].MediaOpened += loadedMusic;

                using (FileStream fs = File.OpenRead(filePath))
                {
                    if (fs.Length >= 128)
                    {
                        MusicID3Tag tag = new MusicID3Tag();
                        fs.Seek(-128, SeekOrigin.End);
                        fs.Read(tag.TAGID, 0, tag.TAGID.Length);
                        fs.Read(tag.Title, 0, tag.Title.Length);
                        fs.Read(tag.Artist, 0, tag.Artist.Length);
                        fs.Read(tag.Album, 0, tag.Album.Length);
                        fs.Read(tag.Year, 0, tag.Year.Length);
                        fs.Read(tag.Comment, 0, tag.Comment.Length);
                        fs.Read(tag.Genre, 0, tag.Genre.Length);
                        string theTAGID = Encoding.Default.GetString(tag.TAGID);

                        string fileNameOnly = Path.GetFileName(filePath);
                        _controls[index].FileNameLabel.Content = fileNameOnly;

                        if (theTAGID.Equals("TAG"))
                        {
                            string Title = Encoding.Default.GetString(tag.Title);
                            string Artist = Encoding.Default.GetString(tag.Artist);
                            string Album = Encoding.Default.GetString(tag.Album);
                            string Year = Encoding.Default.GetString(tag.Year);
                            string Comment = Encoding.Default.GetString(tag.Comment);
                            string Genre = Encoding.Default.GetString(tag.Genre);

                        }
                    }
                }
                _players[index].Play();
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void InitMediaPlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i] = new MediaPlayer();
            }
        }
        private void StopMediaPlayers()
        {
            for(int i = 0; i < _players.Length; i++)
            {
                _players[i].Stop();
            }
        }

        private void MultiOpen_Click(object sender, RoutedEventArgs e)
        {
            

            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            openFileDlg.DefaultExt = ".mp3";
            openFileDlg.Filter = "MP3 file (.mp3)|*.mp3";
            openFileDlg.Multiselect = true;

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                if(openFileDlg.FileNames.Length > 9)
                {
                    MessageBox.Show("알림", "9개까지 재생 가능합니다.");
                    return;
                }
                StopMediaPlayers();
                for (int i = 0; i < openFileDlg.FileNames.Length; i++)
                {
                    
                    filePath = openFileDlg.FileNames[i];

                    //sampleMediaPlayer.Open(new Uri(filePath));
                    //sampleMediaPlayer.MediaOpened += loadedMusic;
                    _players[i].Open(new Uri(filePath));
                    _players[i].MediaOpened += loadedMusic;

                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        if (fs.Length >= 128)
                        {
                            MusicID3Tag tag = new MusicID3Tag();
                            fs.Seek(-128, SeekOrigin.End);
                            fs.Read(tag.TAGID, 0, tag.TAGID.Length);
                            fs.Read(tag.Title, 0, tag.Title.Length);
                            fs.Read(tag.Artist, 0, tag.Artist.Length);
                            fs.Read(tag.Album, 0, tag.Album.Length);
                            fs.Read(tag.Year, 0, tag.Year.Length);
                            fs.Read(tag.Comment, 0, tag.Comment.Length);
                            fs.Read(tag.Genre, 0, tag.Genre.Length);
                            string theTAGID = Encoding.Default.GetString(tag.TAGID);

                            string fileNameOnly = Path.GetFileName(filePath);
                            _controls[i].FileNameLabel.Content = fileNameOnly;

                            if (theTAGID.Equals("TAG"))
                            {
                                string Title = Encoding.Default.GetString(tag.Title);
                                string Artist = Encoding.Default.GetString(tag.Artist);
                                string Album = Encoding.Default.GetString(tag.Album);
                                string Year = Encoding.Default.GetString(tag.Year);
                                string Comment = Encoding.Default.GetString(tag.Comment);
                                string Genre = Encoding.Default.GetString(tag.Genre);

                                //songTitleLabel.Content = (Title);
                                //artistNameLabel.Content = (Artist);
                                //albumTitleLabel.Content = (Album);
                            }
                        }
                    }
                    _players[i].Play();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < _players.Length; i++)
            {
                _players[i].Stop();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].Play();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                _players[i].Pause();
            }
        }
    }
}
