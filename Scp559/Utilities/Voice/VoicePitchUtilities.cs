using Scp559.Utilities.Pooling;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace Scp559.Utilities.Voice;

public static class VoicePitchUtilities
{
    private static readonly float[] ReceivedBuffer = new float[VoiceChatSettings.BufferLength];
    private static readonly byte[] EncodedBuffer = new byte[VoiceChatSettings.MaxEncodedSize];
    
    private static readonly OpusDecoder Decoder = OpusDecoderPool.Shared.Get();
    private static readonly OpusEncoder Encoder = OpusEncoderPool.Shared.Get();
    
    private static readonly PitchShifter PitchShifter = PitchShifterPool.Shared.Get();

    internal static VoiceMessage SetVoicePitch(VoiceMessage msg)
    {
        int decodedData = Decoder.Decode(msg.Data, msg.DataLength, ReceivedBuffer);
        
        PitchShifter.PitchShift(EntryPoint.Instance.Config.CakeConfig.VoicePitch, decodedData, VoiceChatSettings.SampleRate, ReceivedBuffer);
        
        int length = Encoder.Encode(ReceivedBuffer, EncodedBuffer);
        
        msg.Data = EncodedBuffer;
        msg.DataLength = length;
        
        return msg;
    }
}