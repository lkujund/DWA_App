using Integration.BLModels;
using Integration.Models;

namespace Integration.Mapping
{
    public class NotificationMapping
    {
        public static IEnumerable<BLNotification> MapToBL(IEnumerable<Notification> notifications) =>
        notifications.Select(x => MapToBL(x));

        public static BLNotification MapToBL(Notification notification) =>
            new BLNotification
            {
                Id = notification.Id,
                ReceiverEmail= notification.ReceiverEmail,
                Subject= notification.Subject,
                Body=notification.Body
            };

        public static IEnumerable<Notification> MapToDAL(IEnumerable<BLNotification> blVideos)
            => blVideos.Select(x => MapToDAL(x));

        public static Notification MapToDAL(BLNotification notification) =>
            new Notification
            {
                Id = notification.Id,
                CreatedAt = DateTime.Now,
                ReceiverEmail= notification.ReceiverEmail,
                Subject= notification.Subject,
                Body=notification.Body
            };
    }
}
