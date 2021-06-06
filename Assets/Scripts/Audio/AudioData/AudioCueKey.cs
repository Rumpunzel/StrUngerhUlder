using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct AudioCueKey
{
	public static AudioCueKey invalid = new AudioCueKey(-1, null);

	internal int m_Value;
	internal AudioCueSO m_AudioCue;


	public override bool Equals(Object obj)
	{
		return obj is AudioCueKey x && m_Value == x.m_Value && m_AudioCue == x.m_AudioCue;
	}

	public override int GetHashCode()
	{
		return m_Value.GetHashCode() ^ m_AudioCue.GetHashCode();
	}

	public static bool operator ==(AudioCueKey x, AudioCueKey y)
	{
		return x.m_Value == y.m_Value && x.m_AudioCue == y.m_AudioCue;
	}

	public static bool operator !=(AudioCueKey x, AudioCueKey y)
	{
		return !(x == y);
	}


    internal AudioCueKey(int value, AudioCueSO audioCue)
    {
        m_Value = value;
        m_AudioCue = audioCue;
    }
}
