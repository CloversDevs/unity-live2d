namespace Dedalord.LiveAr
{
    /// <summary>
    /// Ids for the control messages that can be sent throughout the app.
    /// WARNING: These are used for Unity serialization, so always assign int values and avoid changing existing ones!
    /// </summary>
    public enum SimpleMessageId
    {
        GO_ROOT = 0,
        GO_LIVE_2D = 1,
        GO_AR_SAMPLES = 2,
        GO_FACE_POSE = 3,
        GO_FACE_MESH = 4,
        GO_EYE_POSE = 5,
        GO_LIP_SYNC = 6,
        GO_CHARACTER = 7,
        GO_PERFORMANCE = 8,
    }
}