using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RoyalPerfume.Models;

namespace RoyalPerfume.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendOrderEmailAsync(Order order)
        {
            var emailPassword = _config["Email:Password"]?.Trim();
            var emailUsername = _config["Email:Username"]?.Trim();
            var emailFrom = _config["Email:From"]?.Trim();

            if (string.IsNullOrEmpty(emailPassword)
                || string.IsNullOrEmpty(emailFrom)
                || string.IsNullOrEmpty(emailUsername))
            {
                _logger.LogWarning(
                    "Email not configured — skipping send for order {OrderNumber}",
                    order.OrderNumber);

                return;
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("أطياب رويال", emailFrom));
                message.To.Add(new MailboxAddress("أطياب رويال - الطلبات", emailUsername));
                message.Subject = $"🎁 طلب جديد #{order.OrderNumber} - {order.FullName}";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = BuildOrderEmailHtml(order)
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _config["Email:Host"] ?? "smtp.gmail.com",
                    int.Parse(_config["Email:Port"] ?? "587"),
                    SecureSocketOptions.StartTls
                );
                await smtp.AuthenticateAsync(emailUsername, emailPassword);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Order email sent successfully for order {OrderNumber}", order.OrderNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send order email for order {OrderNumber}", order.OrderNumber);
            }
        }

        private string BuildOrderEmailHtml(Order order)
        {
            var contactPhone = _config["Contact:Phone"] ?? "01022624475";
            var paymentMethodText = order.PaymentMethod switch
            {
                "cash" => "💵 الدفع عند الاستلام",
                "instapay" => "📱 إنستاباي / فودافون كاش",
                "card" => "💳 البطاقة البنكية",
                _ => order.PaymentMethod
            };

            var itemsHtml = string.Join("", order.Items.Select(item => $"""
                <tr>
                    <td style="padding: 12px; border-bottom: 1px solid #f0e6d3; text-align: right;">
                        <strong style="color: #2c1810;">{item.ProductName}</strong>
                        <br><small style="color: #8b7355;">{item.Volume}</small>
                    </td>
                    <td style="padding: 12px; border-bottom: 1px solid #f0e6d3; text-align: center; color: #5c3d1e;">{item.Quantity}</td>
                    <td style="padding: 12px; border-bottom: 1px solid #f0e6d3; text-align: center; color: #c8922a; font-weight: bold;">{item.Price:N0} ج.م</td>
                    <td style="padding: 12px; border-bottom: 1px solid #f0e6d3; text-align: center; color: #8b4513; font-weight: bold;">{item.TotalPrice:N0} ج.م</td>
                </tr>
            """));

            return $"""
                <!DOCTYPE html>
                <html dir="rtl" lang="ar">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>طلب جديد - أطياب رويال</title>
                </head>
                <body style="margin: 0; padding: 0; background-color: #f5ede0; font-family: 'Segoe UI', Tahoma, Arial, sans-serif; direction: rtl;">
                    
                    <!-- Header -->
                    <div style="background: linear-gradient(135deg, #1a0a00 0%, #3d1a08 50%, #2c1205 100%); padding: 40px 20px; text-align: center;">
                        <h1 style="color: #c8922a; margin: 0; font-size: 32px; letter-spacing: 2px;">🌟 أطياب رويال</h1>
                        <p style="color: #e8c87a; margin: 10px 0 0; font-size: 16px;">طلب جديد وصل!</p>
                        <div style="background: rgba(200, 146, 42, 0.2); border: 1px solid #c8922a; border-radius: 25px; display: inline-block; padding: 8px 25px; margin-top: 15px;">
                            <span style="color: #ffd700; font-size: 18px; font-weight: bold;">طلب رقم #{order.OrderNumber}</span>
                        </div>
                    </div>

                    <!-- Main Content -->
                    <div style="max-width: 700px; margin: 30px auto; padding: 0 20px;">
                        
                        <!-- Order Date -->
                        <div style="background: white; border-radius: 12px; padding: 15px 25px; margin-bottom: 20px; text-align: center; box-shadow: 0 2px 15px rgba(0,0,0,0.08);">
                            <p style="margin: 0; color: #666; font-size: 14px;">📅 تاريخ الطلب: <strong style="color: #2c1810;">{order.OrderDate:yyyy/MM/dd - hh:mm tt}</strong></p>
                        </div>

                        <!-- Customer Info -->
                        <div style="background: white; border-radius: 16px; padding: 25px; margin-bottom: 20px; box-shadow: 0 4px 20px rgba(0,0,0,0.08); border-right: 5px solid #c8922a;">
                            <h2 style="color: #2c1810; margin: 0 0 20px; font-size: 20px; display: flex; align-items: center; gap: 10px;">
                                👤 بيانات العميل
                            </h2>
                            <table style="width: 100%; border-collapse: collapse;">
                                <tr>
                                    <td style="padding: 10px 0; color: #8b7355; width: 40%; vertical-align: top;">الاسم الكامل:</td>
                                    <td style="padding: 10px 0; color: #2c1810; font-weight: bold; font-size: 18px;">{order.FullName}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 0; color: #8b7355; vertical-align: top;">رقم الهاتف:</td>
                                    <td style="padding: 10px 0; color: #2c1810; font-weight: bold; font-size: 16px;">{order.Phone}</td>
                                </tr>
                                {(string.IsNullOrEmpty(order.AlternatePhone) ? "" : $@"
                                <tr>
                                    <td style=""padding: 10px 0; color: #8b7355; vertical-align: top;"">هاتف بديل:</td>
                                    <td style=""padding: 10px 0; color: #2c1810;"">{order.AlternatePhone}</td>
                                </tr>")}
                                <tr>
                                    <td style="padding: 10px 0; color: #8b7355; vertical-align: top;">المحافظة:</td>
                                    <td style="padding: 10px 0; color: #2c1810;">{order.Governorate} {(!string.IsNullOrEmpty(order.District) ? "- " + order.District : "")}</td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 0; color: #8b7355; vertical-align: top;">العنوان:</td>
                                    <td style="padding: 10px 0; color: #2c1810;">{order.Address}</td>
                                </tr>
                                {(string.IsNullOrEmpty(order.Notes) ? "" : $@"
                                <tr>
                                    <td style=""padding: 10px 0; color: #8b7355; vertical-align: top;"">ملاحظات:</td>
                                    <td style=""padding: 10px 0; color: #555; font-style: italic;"">{order.Notes}</td>
                                </tr>")}
                            </table>
                        </div>

                        <!-- Payment Method -->
                        <div style="background: linear-gradient(135deg, #fff8f0, #fff3e6); border-radius: 12px; padding: 18px 25px; margin-bottom: 20px; border: 1px solid #f0d0a0; text-align: center;">
                            <h3 style="margin: 0; color: #5c3d1e; font-size: 18px;">طريقة الدفع: <span style="color: #c8922a;">{paymentMethodText}</span></h3>
                        </div>

                        <!-- Products Table -->
                        <div style="background: white; border-radius: 16px; padding: 25px; margin-bottom: 20px; box-shadow: 0 4px 20px rgba(0,0,0,0.08);">
                            <h2 style="color: #2c1810; margin: 0 0 20px; font-size: 20px;">🛒 المنتجات المطلوبة</h2>
                            <table style="width: 100%; border-collapse: collapse;">
                                <thead>
                                    <tr style="background: linear-gradient(135deg, #2c1810, #5c3d1e);">
                                        <th style="padding: 14px 12px; color: #f5e6d0; text-align: right; border-radius: 8px 0 0 0;">المنتج</th>
                                        <th style="padding: 14px 12px; color: #f5e6d0; text-align: center;">الكمية</th>
                                        <th style="padding: 14px 12px; color: #f5e6d0; text-align: center;">السعر</th>
                                        <th style="padding: 14px 12px; color: #f5e6d0; text-align: center; border-radius: 0 8px 0 0;">الإجمالي</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {itemsHtml}
                                </tbody>
                            </table>
                        </div>

                        <!-- Order Summary -->
                        <div style="background: linear-gradient(135deg, #2c1810, #1a0a00); border-radius: 16px; padding: 25px; margin-bottom: 20px; color: white;">
                            <h2 style="color: #c8922a; margin: 0 0 20px; font-size: 20px;">💰 ملخص الفاتورة</h2>
                            <table style="width: 100%;">
                                <tr>
                                    <td style="padding: 10px 0; color: #d4a574; font-size: 16px;">المجموع الفرعي:</td>
                                    <td style="padding: 10px 0; color: white; text-align: left; font-size: 16px;">{order.Subtotal:N0} ج.م</td>
                                </tr>
                                {(order.Discount > 0 ? $@"
                                <tr>
                                    <td style=""padding: 10px 0; color: #90ee90; font-size: 16px;"">الخصم {(string.IsNullOrEmpty(order.CouponCode) ? "" : $"(كوبون: {order.CouponCode})")}:</td>
                                    <td style=""padding: 10px 0; color: #90ee90; text-align: left; font-size: 16px;"">- {order.Discount:N0} ج.م</td>
                                </tr>" : "")}
                                <tr>
                                    <td style="padding: 10px 0; color: #d4a574; font-size: 16px;">الشحن:</td>
                                    <td style="padding: 10px 0; color: #90ee90; text-align: left; font-size: 16px;">{(order.ShippingCost == 0 ? "مجاني 🎁" : order.ShippingCost.ToString("N0") + " ج.م")}</td>
                                </tr>
                                <tr style="border-top: 2px solid #c8922a;">
                                    <td style="padding: 15px 0 5px; color: #ffd700; font-size: 22px; font-weight: bold;">الإجمالي للدفع:</td>
                                    <td style="padding: 15px 0 5px; color: #ffd700; text-align: left; font-size: 26px; font-weight: bold;">{order.Total:N0} ج.م</td>
                                </tr>
                            </table>
                        </div>

                        <!-- Footer -->
                        <div style="text-align: center; padding: 20px; color: #8b7355;">
                            <p style="margin: 0 0 10px; font-size: 14px;">📞 للتواصل: {contactPhone}</p>
                            <p style="margin: 0; font-size: 12px; color: #b0a090;">أطياب رويال - نبض فخامة العطر الأصيل</p>
                        </div>

                    </div>
                </body>
                </html>
            """;
        }
    }
}
