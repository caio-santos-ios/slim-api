using api_slim.src.Configuration;
using api_slim.src.Interfaces;
using api_slim.src.Models;
using api_slim.src.Models.Base;
using MongoDB.Driver;

namespace api_slim.src.Repository
{
    public class DashboardRepository(AppDbContext context) : IDashboardRepository
    {
        public async Task<ResponseApi<dynamic>> GetFirstCardAsync()
        {
            try
            {
                DateTime now = DateTime.UtcNow;

                DateTime startOfMonth = new(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
                
                DateTime startOfYear = new(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime endOfYear = startOfYear.AddYears(1).AddTicks(-1);

                DateTime previousStartDate = startOfMonth.AddMonths(-1);
                DateTime previousEndDate = endOfMonth.AddMonths(-1);
                
                DateTime previousStartOfYear = startOfYear.AddYears(-1);
                DateTime previousEndOfYear = endOfYear.AddYears(-1);

                List<AccountsReceivable> accountReceivablePreviuosMonth = await context.AccountsReceivables.Find(x => !x.Deleted && x.BillingDate >= previousStartDate && x.BillingDate <= previousEndDate).ToListAsync();
                List<AccountsReceivable> accountReceivableMonth = await context.AccountsReceivables.Find(x => !x.Deleted && x.BillingDate >= startOfMonth && x.BillingDate <= endOfMonth).ToListAsync();
                
                
                List<AccountsReceivable> accountReceivablePreviousYear = await context.AccountsReceivables.Find(x => !x.Deleted && x.BillingDate >= previousStartOfYear && x.BillingDate <= previousEndOfYear).ToListAsync();
                List<AccountsReceivable> accountReceivableYear = await context.AccountsReceivables.Find(x => !x.Deleted && x.BillingDate >= startOfYear && x.BillingDate <= endOfYear).ToListAsync();
                
                long customer = await context.Customers.Find(x => !x.Deleted).CountDocumentsAsync();
                long recipient = await context.CustomerRecipients.Find(x => !x.Deleted).CountDocumentsAsync();

                decimal totalPrevious = accountReceivablePreviuosMonth.Sum(x => x.Value);
                decimal month = accountReceivableMonth.Sum(x => x.Value);
                decimal previousYear = accountReceivablePreviousYear.Sum(x => x.Value);
                decimal year = accountReceivableYear.Sum(x => x.Value);

                decimal percentageChangeMonth = 0;
                decimal percentageChangeYear = 0;

                if (totalPrevious > 0)
                {
                    percentageChangeMonth = ((month - totalPrevious) / totalPrevious) * 100;
                }
                else if (month > 0)
                {
                    percentageChangeMonth = 100;
                }
                
                if (previousYear > 0)
                {
                    percentageChangeYear = ((year - previousYear) / previousYear) * 100;
                }
                else if (year > 0)
                {
                    percentageChangeYear = 100;
                }

                dynamic obj = new
                {
                    accountReceivableMonth = month,
                    percentageChangeMonth,
                    accountReceivableYear = year,
                    percentageChangeYear,
                    customer,
                    recipient
                };

                return new(obj);
            }
            catch
            {
                return new(null, 500, "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde."); ;
            }
        }
    }
}