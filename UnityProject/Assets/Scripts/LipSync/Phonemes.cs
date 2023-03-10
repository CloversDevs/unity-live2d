using System.Collections.Generic;

namespace Dedalord.LiveAr
{
    public partial class TextToVisemes
    {
        /// <summary>
        /// Map from names of phonemes to the Viseme animation ids.
        /// </summary>
        public static readonly Dictionary<string, Viseme> _phonemeToViseme = new()
        {
            { "AH", Viseme.A },
            { "EY", Viseme.E }, /// seguido de Viseme.I
            { "Z", Viseme.C_D_G_K_N_S_X_Z },
            { "D", Viseme.C_D_G_K_N_S_X_Z },
            { "IY", Viseme.I },
            { "EH", Viseme.E },
            { "M", Viseme.B_M_P },
            { "F", Viseme.F_T_Th_V },
            { "AO", Viseme.O },
            { "R", Viseme.R_L },
            { "T", Viseme.F_T_Th_V },
            { "UW", Viseme.U },
            { "W", Viseme.U },
            { "N", Viseme.C_D_G_K_N_S_X_Z },
            { "IH", Viseme.I },
            { "P", Viseme.B_M_P },
            { "L", Viseme.R_L },
            { "AY", Viseme.A }, /// seguido de Viseme.I
            { "AA", Viseme.O },
            { "B", Viseme.B_M_P },
            { "ER", Viseme.E },
            { "G", Viseme.C_D_G_K_N_S_X_Z },
            { "K", Viseme.C_D_G_K_N_S_X_Z },
            { "S", Viseme.C_D_G_K_N_S_X_Z },
            { "TH", Viseme.F_T_Th_V },
            { "V", Viseme.F_T_Th_V },
            { "HH", Viseme.J_Ch_Sh },
            { "AE", Viseme.A },
            { "OW", Viseme.O },
            { "NG", Viseme.C_D_G_K_N_S_X_Z },
            { "SH", Viseme.J_Ch_Sh },
            { "ZH", Viseme.J_Ch_Sh },
            { "Y", Viseme.I },
            { "AW", Viseme.A },
            { "JH", Viseme.C_D_G_K_N_S_X_Z },
            { "CH", Viseme.J_Ch_Sh },
            { "UH", Viseme.U },
            { "DH", Viseme.C_D_G_K_N_S_X_Z },
            { "OY", Viseme.O } /// seguido de Viseme.I
        };
    }
}