using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Windows.UI.Notifications;

namespace MediaCheck.Services.Helpers {
    class NotificationService {
        public static void sendNotificationToast(string state, string title, string imageURL) {
            ToastVisual visual = new ToastVisual() {
                BindingGeneric = new ToastBindingGeneric() {
                    Children =  {
                        new AdaptiveText(){
                            Text = state
                        },
                        new AdaptiveText(){
                            Text = title
                        }
                    },
                    AppLogoOverride = new ToastGenericAppLogo() {
                        Source = imageURL,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }
                }
            };
            ToastContent content = new ToastContent() {
                Visual = visual
            };
            var toast = new ToastNotification(content.GetXml());
            toast.ExpirationTime = DateTime.Now.AddSeconds(5);
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(toast);
        }

        public static void scheduleToast(string state, string title, string imageURL, DateTime date) {
            ToastVisual visual = new ToastVisual() {
                BindingGeneric = new ToastBindingGeneric() {
                    Children =  {
                            new AdaptiveText(){
                                Text = state
                            },
                            new AdaptiveText(){
                                Text = title
                            }
                        },
                    AppLogoOverride = new ToastGenericAppLogo() {
                        Source = imageURL,
                        HintCrop = ToastGenericAppLogoCrop.Circle
                    }
                }
            };
            ToastContent content = new ToastContent() {
                Visual = visual
            };
            var toast = new ScheduledToastNotification(content.GetXml(), date);
            toast.Tag = title;
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.AddToSchedule(toast);
        }

        public static void setTile(string title, string imageURL, DateTime dateTime) {
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            string date;
            if(dateTime.Date == DateTime.Today) {
                date = "Dziś";
            } else {
                date = dateTime.Date.ToString("D");
            }

            TileContent content = new TileContent() {
                Visual = new TileVisual() {
                    TileMedium = new TileBinding() {
                        Content = new TileBindingContentAdaptive() {
                            BackgroundImage = new TileBackgroundImage() {
                                Source = imageURL
                            },
                            Children = {
                                new AdaptiveText(){
                                    Text = title
                                },
                                new AdaptiveText(){
                                    Text = date/*,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle*/
                                },
                                new AdaptiveText(){
                                    Text = dateTime.TimeOfDay.ToString()/*,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle*/
                                }
                            }
                        }
                    },
                    TileWide = new TileBinding() {
                        Content = new TileBindingContentAdaptive() {
                            BackgroundImage = new TileBackgroundImage() {
                                Source = imageURL
                            },
                            Children = {
                                new AdaptiveText(){
                                    Text = title
                                },
                                new AdaptiveText(){
                                    Text = date/*,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle*/
                                },
                                new AdaptiveText(){
                                    Text = dateTime.TimeOfDay.ToString()/*,
                                    HintStyle = AdaptiveTextStyle.CaptionSubtle*/
                                }
                            }
                        }
                    }
                }
            };

            var notification = new TileNotification(content.GetXml());
            notification.Tag = title;
            notification.ExpirationTime = dateTime;
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}
