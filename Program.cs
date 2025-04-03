using NAudio.Wave;
using System;
using System.IO;
using System.Timers;
namespace AutomaticVoiceRecording
{
    internal class Program
    {
        private static WaveInEvent waveSource;
        private static WaveFileWriter waveFile;
        private static string outputFilePath = "recordedAudio";
        private static bool isRecording = false;
        private static int silenceDuration = 1000; 
        private static int silenceThreshold = 500; 
        static System.Timers.Timer silenceTimer;

        static void Main(string[] args)
        {
            if(!Directory.Exists(outputFilePath))
            {
                Directory.CreateDirectory(outputFilePath);
            }
            waveSource = new WaveInEvent();
            waveSource.WaveFormat = new WaveFormat(44100, 1);
            waveSource.DataAvailable += waveSource_Dataavailble;
            waveSource.RecordingStopped += waveSource_RecordingStoped;
            waveSource.StartRecording();
            silenceTimer = new System.Timers.Timer(silenceDuration);
            silenceTimer.Elapsed += Elapsed;
            Console.WriteLine("Listenig the microphone activity....... Press Enter To Exit");
            Console.ReadLine();
           waveSource.StopRecording();
            waveSource.Dispose();




        }
        private static void waveSource_Dataavailble(object sender,WaveInEventArgs e)
        {
            float Level = GetAudioLevel(e.Buffer);
            if(Level > silenceThreshold)
            {
                if (!isRecording)
                {
                    StartRecording();
                }
                silenceTimer.Stop();
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();


            }
            else if (isRecording)
            {
                silenceTimer.Start();
            }

        }
        private static void StartRecording()
        {
            string FileName = $"{outputFilePath}/Recording_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.wav";
            waveFile = new WaveFileWriter(FileName, waveSource.WaveFormat);
        
            isRecording = true;
            Console.WriteLine($"Recording started fileName {FileName}");
        }

        private static void Elapsed(object sender, ElapsedEventArgs e)
        {
            StopRecording();
        }
        private static void StopRecording()
        {
            if(!isRecording)
            {
                return;
            }
            isRecording = false;
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
            Console.WriteLine("Recording stopped");
        }
        private static void waveSource_RecordingStoped(object sender, StoppedEventArgs e)
        {
            StopRecording();
        }

        private static float GetAudioLevel(byte[] buffer)
        {
            int max = 0;
            for (int index = 0; index < buffer.Length; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index]);
                max = Math.Max(max, Math.Abs(sample));
            }
            return max;
        }
    }
}
