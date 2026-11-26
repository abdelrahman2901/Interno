using System.Globalization;
using E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo.Models;
using E_Commerce_Interno.Domain.Entity;
using E_Commerce_Interno.Domain.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace E_Commerce_Inerno.Infrastructure.Repository.PaymentRepo
{
    /// <summary>
    /// Builds a branded Driver Finder payment invoice PDF (in-memory only).
    /// </summary>
    public static class PaymentInvoicePdfBuilder
    {
        private static readonly Color Navy = Color.FromHex("#0F233F");
        private static readonly Color AccentRed = Color.FromHex("#E53535");
        private static readonly Color SoftBg = Color.FromHex("#F5F7FA");
        private static readonly Color Line = Color.FromHex("#E2E8F0");
        private static readonly Color Muted = Color.FromHex("#64748B");
        private static readonly Color ValueColor = Color.FromHex("#1E293B");

        static PaymentInvoicePdfBuilder()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public static byte[] Build(
            Payments payment,
            PaymentInvoiceLabels labels,
            byte[]? logoBytes,
            CultureInfo culture)
        {
            var isRtl = culture.TextInfo.IsRightToLeft;
            var invoiceNumber = payment.PaymentID.ToString("N")[..8].ToUpperInvariant();
            var userName = string.IsNullOrWhiteSpace(payment.User?.PersonName)
                ? (payment.User?.Email ?? "—")
                : payment.User!.PersonName!;
            var amount = payment.Amount.ToString("N2", culture);
            var date = payment.PaymentDate.ToString("dd MMM yyyy, HH:mm", culture);
            var method = LocalizePaymentMethod(payment.PaymentMethod, culture);
            var status = LocalizeStatus(payment.Status, culture);
            var fontFamily = ResolveFontFamily(isRtl);
            var generatedAt = DateTime.UtcNow.ToString("dd MMM yyyy HH:mm", culture);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x
                        .FontFamily(fontFamily)
                        .FontSize(11)
                        .FontColor(ValueColor));

                    if (isRtl)
                    {
                        page.ContentFromRightToLeft();
                    }

                    page.Background().Background(QuestPDF.Helpers.Colors.White);

                    page.Header().Background(Navy).Padding(28).Row(row =>
                    {
                        row.RelativeItem().Column(col =>
                        {
                            if (logoBytes is { Length: > 0 })
                            {
                                col.Item().Height(52).Width(180).Image(logoBytes).FitArea();
                            }
                            else
                            {
                                col.Item().Text(labels.BrandName).Bold().FontSize(22).FontColor(QuestPDF.Helpers.Colors.White);
                            }

                            col.Item().PaddingTop(10)
                                .Text(labels.Title)
                                .FontSize(13)
                                .FontColor(QuestPDF.Helpers.Colors.White);
                        });

                        row.ConstantItem(150).AlignMiddle().AlignRight().Column(col =>
                        {
                            col.Item().AlignRight().Text(labels.InvoiceNumber)
                                .FontSize(9)
                                .FontColor(QuestPDF.Helpers.Colors.White);
                            col.Item().AlignRight().Text(invoiceNumber)
                                .Bold()
                                .FontSize(16)
                                .FontColor(QuestPDF.Helpers.Colors.White);
                        });
                    });

                    page.Content().Padding(28).Column(col =>
                    {
                        col.Spacing(16);

                        col.Item().Background(SoftBg).Border(1).BorderColor(Line).Padding(18).Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text(labels.Customer).FontSize(9).FontColor(Muted);
                                c.Item().PaddingTop(4).Text(userName).SemiBold().FontSize(13);
                            });

                            row.ConstantItem(160).Background(QuestPDF.Helpers.Colors.White).Border(1).BorderColor(Line).Padding(12).Column(c =>
                            {
                                c.Item().Text(labels.Amount).FontSize(9).FontColor(Muted);
                                c.Item().PaddingTop(4).Text($"{amount} EGP")
                                    .Bold()
                                    .FontSize(18)
                                    .FontColor(AccentRed);
                            });
                        });

                        col.Item().Text(labels.Title).SemiBold().FontSize(12).FontColor(Navy);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(2f);
                            });

                            void Row(string label, string value, bool highlight = false)
                            {
                                var bg = highlight ? SoftBg : QuestPDF.Helpers.Colors.White;

                                table.Cell().Background(bg).BorderBottom(1).BorderColor(Line)
                                    .PaddingVertical(10).PaddingHorizontal(8)
                                    .Text(label).SemiBold().FontColor(Muted).FontSize(10);

                                table.Cell().Background(bg).BorderBottom(1).BorderColor(Line)
                                    .PaddingVertical(10).PaddingHorizontal(8)
                                    .Text(value).FontColor(ValueColor).FontSize(11);
                            }

                            Row(labels.Date, date, true);
                            Row(labels.PaymentMethod, method);
                            Row(labels.Status, status, true);
                            
                        });

                        col.Item().PaddingTop(8).Text(labels.ThankYou)
                            .FontSize(10)
                            .FontColor(Muted);
                    });

                    page.Footer().BorderTop(2).BorderColor(AccentRed).Padding(16).AlignCenter().Text(text =>
                    {
                        text.Span($"{labels.GeneratedBy}  •  ").FontSize(9).FontColor(Muted);
                        text.Span(generatedAt).FontSize(9).FontColor(Muted);
                    });
                });
            }).GeneratePdf();
        }

        private static string ResolveFontFamily(bool isRtl)
        {
            // Segoe UI / Tahoma cover Arabic glyphs on Windows hosts.
            // Fall back to Arial which is commonly available on Linux containers too.
            if (isRtl)
            {
                if (OperatingSystem.IsWindows())
                {
                    return "Segoe UI";
                }

                return "Arial";
            }

            return "Segoe UI";
        }

        private static string LocalizePaymentMethod(PaymentMethods? method, CultureInfo culture)
        {
            if (method is null)
            {
                return "—";
            }

            var ar = culture.TwoLetterISOLanguageName.Equals("ar", StringComparison.OrdinalIgnoreCase);
            return method switch
            {
                PaymentMethods.Wallet => ar ? "المحفظة" : "Wallet",
                PaymentMethods.CreditCard => ar ? "بطاقة ائتمان" : "Credit Card",
                _ => method.ToString()!
            };
        }

        private static string LocalizeStatus(StatusEnum status, CultureInfo culture)
        {
            var ar = culture.TwoLetterISOLanguageName.Equals("ar", StringComparison.OrdinalIgnoreCase);
            return status switch
            {
                StatusEnum.Approved => ar ? "مقبول" : "Approved",
                StatusEnum.Pending => ar ? "قيد المراجعة" : "Pending",
                StatusEnum.Rejected => ar ? "مرفوض" : "Rejected",
                _ => status.ToString()
            };
        }
    }
}
