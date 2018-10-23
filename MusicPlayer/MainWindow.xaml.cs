using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Drawing;
using System.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using CSCore;
using CSCore.Codecs;
using CSCore.CoreAudioAPI;
using CSCore.SoundOut;
using System.Data;

namespace MusicPlayer
{
    public class AudioPlayer : Component
    {
        private ISoundOut soundOut;
        private IWaveSource waveSource;
        public AudioPlayer()
        {
            
        }

        public void Open(string fileName, MMDevice device)
        {
//            fileName = "G:\\colorful.mp3";
            CleanupPlayback();
            waveSource =
                CodecFactory.Instance.GetCodec(fileName)
                    .ToSampleSource()
                    .ToMono()
                    .ToWaveSource();
            soundOut = new WasapiOut() { Latency = 100, Device = device };
            soundOut.Initialize(waveSource);
//            if (PlaybackStopped != null) soundOut.Stopped += PlaybackStopped;
        }
        public void Play()
        {
            if (soundOut != null)
                soundOut.Play();
        }

        public void Pause()
        {
            if (soundOut != null)
                soundOut.Pause();
        }

        public void Stop()
        {
            if (soundOut != null)
                soundOut.Stop();
        }
        private void CleanupPlayback()
        {
            if (soundOut != null)
            {
                soundOut.Dispose();
                soundOut = null;
            }
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CleanupPlayback();
        }

    }
    public class MusicInfo : INotifyPropertyChanged
    {
        private string mTitle = "hehehe";
        private string mAlbum;
        private string mArtist;
        private string mFullName;
        private string mPath;
        public event PropertyChangedEventHandler PropertyChanged;
        public MusicInfo()
        {
        }

        public string Title
        {
            get
            {
                return mTitle;
            }

            set
            {
                mTitle = value;

//                if (PropertyChanged != null)
//                {
//                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Title"));
//                }
            }
        }

        public string Album
        {
            get
            {
                return mAlbum;
            }

            set
            {
                mAlbum = value;
            }
        }

        public string Artist
        {
            get
            {
                return mArtist;
            }

            set
            {
                mArtist = value;
            }
        }

        public string MusicFullName
        {
            get
            {
                return mFullName;
            }

            set
            {
                mFullName = Artist + "-" + Title + "-" + Album;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MusicFullName"));
                }
            }
        }

        public string MusicPath
        {
            get
            {
                return mPath;
            }

            set
            {
                mPath = value;
            }
        }
    }
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MusicInfo _myMusicTest;
        private AudioPlayer myAudioPlayer;
        private SoundPlayer MyPlayer;
        private MMDevice dev;
        public MainWindow()
        {
            MyMusicTest = new MusicInfo();
            MyPlayer = new SoundPlayer();
            myAudioPlayer = new AudioPlayer();
            this.DataContext = this;
            InitializeComponent();
            InitMusicInfo();
        }

        public MusicInfo MyMusicTest
        {
            get
            {
                return _myMusicTest;
            }

            set
            {
                _myMusicTest = value;
            }
        }

        private void InitMusicInfo()
        {
            TagLib.File f = TagLib.File.Create("G:/afternoon.mp3");
            if (!string.IsNullOrEmpty(f.Tag.FirstPerformer))
                MyMusicTest.Artist = f.Tag.FirstPerformer;
            else

                MyMusicTest.Artist = "";


            if (!string.IsNullOrEmpty(f.Tag.Title))
                MyMusicTest.Title = f.Tag.Title;
            if (!string.IsNullOrEmpty(f.Tag.Album))
            {
                MyMusicTest.Album = f.Tag.Album;
            }

            MyMusicTest.MusicPath = f.Name;
            MyPlayer.SoundLocation = f.Name;
//            MyPlayer.Load();
            MyMusicTest.MusicFullName = "";
            Uri xUri = new Uri(f.Name);
//            mediaElement.Source = xUri;
            string timeLength = "";
//            if (f.Properties.Duration.ToString().Length != 0)
//            {
//                timeLength = f.Properties.Duration.ToString();
//            }
//            var streamByte = f.Tag.Pictures[0].Data.Data;
//            System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
//            //            var img = System.Drawing.Image.FromStream(ms);
//            BitmapImage bitImg = new BitmapImage();
//            bitImg.BeginInit();
//            bitImg.StreamSource = ms;
//            bitImg.EndInit();
//            MusicPic.Source = bitImg;
        }
        private DispatcherTimer timer = null;

        private void OnClickMe(object sender, RoutedEventArgs e)
        {
            MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
            dev = DevEnum.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
//            var mmdeviceCollection = DevEnum.EnumAudioEndpoints(DataFlow.Render, DeviceState.All);
//            dev = mmdeviceCollection.First();
            //this cause crash
            myAudioPlayer.Open("G:/afternoon.mp3", dev);
            myAudioPlayer.Play();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
        }

        private void MediaElement_OnMediaOpened(object sender, RoutedEventArgs e)
        {
            MusicProgress.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
//            timer = new DispatcherTimer();
//            timer.Interval = TimeSpan.FromSeconds(1);
//            timer.Tick += new EventHandler(timer_tick);
//            timer.Start();
        }
        private void timer_tick(object sender, EventArgs e)
        {
            MusicProgress.Value = 100;
        }
        private void MusicProgress_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(MusicProgress.Value);
        }

        private void MediaElement_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var ss = e.ErrorException.ToString();
            int xx = 0;
        }
    }
}
