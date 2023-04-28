using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

    public class VideoEventArgs : EventArgs
    {
        public Video? VideoName { get; set; }
    }

    public class Video
    {
        public string? Title { get; set; }

    }
    class VideoEncode
    {
        //public delegate void VideoEncoderHandler(object source , VideoEventArgs args);
        public event EventHandler<VideoEventArgs>? VideoEncoder;
        public void Encode(Video video)
        {
            Console.WriteLine("Encoding Video ........");
            Thread.Sleep(3000);
            OnVideoEncoded(video);
        }

        protected virtual void OnVideoEncoded(Video video)
        {
            if(VideoEncoder != null)
            {
                VideoEncoder(this, new VideoEventArgs() { VideoName = video});
            }
        }
    }

    public class MailService
    {
        public void OnVideoEncoding(object sender,VideoEventArgs e)
        {
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + ("Mail Service : Mail Sent successfully!".Length / 2)) + "}", "Mail Service : Mail Sent successfully!"));

        }
    }
    class MainVideo
    {
        
        public static void Main(string[] args)
        {
            Video video = new Video() { Title = "Video 1"};
            VideoEncode videoEncode = new VideoEncode(); // Publiser
            MailService mailService = new MailService();

            videoEncode.VideoEncoder += mailService.OnVideoEncoding;
            videoEncode.Encode(video);
            
        }
    }
}
