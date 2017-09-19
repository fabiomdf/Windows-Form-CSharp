using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.IO;

namespace Persistencia.NSS
{
    public class NSS_Arquivo_WAV
    {
        //        public void Mp3ToWav(string _inPath_, string _outPath_)
        //{
        //    using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
        //    {
        //        GerarNovoFormatoWavePCM(mp3, _outPath_);
        //    }
        //}
        //public void GerarNovoFormatoWavePCM(WaveStream waveOrigin, string _outPath_)
        //{
        //    WaveFormat formatoSaida = new WaveFormat(11025, 8, 1);


        //    if (waveOrigin.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
        //    {
        //        WaveFormatConversionStream wfcs = new WaveFormatConversionStream(formatoSaida, waveOrigin);
        //        using (var conversionStream = wfcs)
        //        {
        //            WaveFileWriter.CreateWaveFile(_outPath_, conversionStream);
        //        }
        //    }
        //    else
        //    {
        //        using (var converter = WaveFormatConversionStream.CreatePcmStream(waveOrigin))
        //        {                    
        //            string outPathTemp = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + Recursos.Caminhos.ArquivoWaveTemp;
        //            WaveFileWriter.CreateWaveFile(outPathTemp, converter);
        //            WaveToWave(outPathTemp, _outPath_);
        //            File.Delete(outPathTemp);
        //        }
        //    }
        //}
        //public void WaveToWave(string _inPath_, string _outPath_)
        //{            
        //    using (WaveFileReader waveOrigin = new WaveFileReader(_inPath_))
        //    {
        //        GerarNovoFormatoWavePCM(waveOrigin, _outPath_);
        //    }
        //}
        public bool VerificarFormatoWavePCM(string _inPath_)
        {
            using (WaveFileReader waveOrigin = new WaveFileReader(_inPath_))
            {
                try
                {
                    WaveFormat formatoSaida = new WaveFormat(11025, 8, 1);

                    return ((waveOrigin.WaveFormat.Encoding == WaveFormatEncoding.Pcm) &&
                        (waveOrigin.WaveFormat.SampleRate == formatoSaida.SampleRate) &&
                        (waveOrigin.WaveFormat.BitsPerSample == formatoSaida.BitsPerSample) &&
                        (waveOrigin.WaveFormat.Channels == formatoSaida.Channels));
                }
                catch
                {
                    return false;
                }
                finally
                {
                    waveOrigin.Close();
                }
            }
        }
    }
}

