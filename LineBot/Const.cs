namespace LineBot
{
    public static class ChannelId
    {
        public static readonly string ReceivingChannelId = "1341301815";
        public static readonly string ReceivingChannelMid = "u206d25c2ea6bd87c17655609a1c37cb8";
        public static readonly string SendingChannelId = "1383378250";
    }

    public static class HeaderName
    {
        public static readonly string ChannelId = "X-Line-ChannelID";
        public static readonly string ChannelSecret = "X-Line-ChannelSecret";
        public static readonly string TrustedUserWithAcl = "X-Line-Trusted-User-With-ACL";
        public static readonly string ChannelToken = "X-Line-ChannelToken";
        public static readonly string ChannelSignature = "X-LINE-ChannelSignature";
    }

    public enum ContentType
    {
        Text = 1,
        Image = 2,
        Video = 3,
        Audio = 4,
        Location = 7,
        Sticker = 8,
        Contact = 10,
        RichMessage = 12
    }

    public static class Endpoint
    {
        public static readonly string TrialBotApi = "https://trialbot-api.line.me";
        public static readonly string BusinessConnect = "https://api.line.me";
    }

    public static class Api
    {
        public static readonly string Event = "/v1/events";
        public static readonly string Profile = "/v1/profiles";
        public static readonly string Content = "/v1/bot/message/<messageId>/content";
        public static readonly string Preview = "/v1/bot/message/<messageId>/content/preview";
    }

    public static class EventType
    {
        public static readonly string ReceivingMessage = "138311609000106303";
        public static readonly string ReceivingOperation = "138311609100106403";
        public static readonly string SendingMessage = "138311608800106203";
        public static readonly string SendingMultipleMessages = "140177271400161403";
    }

    public enum OpType
    {
        AddedAsFriend = 4,
        InvitedToGroup = 5,
        AddedToRoom = 7,
        Blocked = 8
    }

    public enum RecipientType
    {
        User = 1,
        Room = 2,
        Group = 3
    }
}