using Scp559.Utilities.Pooling;
using System;
using System.Collections.Generic;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Networking;

namespace Scp559.Utilities.Voice;

public static class VoicePitchUtilities
{
    private sealed class StreamState
    {
        internal readonly float[] Received = new float[VoiceChatSettings.BufferLength];
        internal readonly byte[] Encoded = new byte[VoiceChatSettings.MaxEncodedSize];
        internal readonly OpusDecoder Decoder = new OpusDecoder();
        internal readonly OpusEncoder Encoder = new OpusEncoder(VoiceChat.Codec.Enums.OpusApplicationType.Voip);
        internal readonly PitchShifter Shifter = new PitchShifter();
    }
    private static readonly Dictionary<ReferenceHub, StreamState> Streams = new();

    internal static void Forget(ReferenceHub hub)
    {
        if (!Streams.TryGetValue(hub, out var stream)) return;
        ((object)stream.Decoder as IDisposable)?.Dispose();
        ((object)stream.Encoder as IDisposable)?.Dispose();
        Streams.Remove(hub);
    }

    internal static VoiceMessage SetVoicePitch(ReferenceHub hub, VoiceMessage msg)
    {
        if (!Streams.TryGetValue(hub, out var stream)) Streams[hub] = stream = new StreamState();
        int decodedData = stream.Decoder.Decode(msg.Data, msg.DataLength, stream.Received);
        
        stream.Shifter.PitchShift(EntryPoint.Instance.Config.CakeConfig.VoicePitch, decodedData, VoiceChatSettings.SampleRate, stream.Received);
        
        int length = stream.Encoder.Encode(stream.Received, stream.Encoded);
        
        msg.Data = new byte[length];
        Array.Copy(stream.Encoded, msg.Data, length);
        msg.DataLength = length;
        
        return msg;
    }
}
