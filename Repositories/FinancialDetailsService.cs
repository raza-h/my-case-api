using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyCaseApi.Dtos;
using MyCaseApi.Entities;
using MyCaseApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCaseApi.Repositories
{
    public class FinancialDetailsService : IFinancialDetailsService
    {
        private readonly ApiDbContext dbContext;
        private readonly IMapper mapper;
        public FinancialDetailsService(ApiDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task<int> AddFinancialDetailsAsync(FinancialDetails financialDetails)
        {
            try
            {
                string newAcode = string.Empty;
                var list = dbContext.FinancialDetails.ToList().Where(x => x.Type == financialDetails.Type);
                if (list != null && list.Count() > 0 && !string.IsNullOrEmpty(list.LastOrDefault().AccountNumber))
                {
                    var splitedCode = list.LastOrDefault().AccountNumber.Split('-');
                    int lastPartOfCode = Convert.ToInt32(splitedCode[3]) + 1;
                    newAcode = financialDetails.Type == Usertype.Customer ? "01-001-0001-000" + Convert.ToString(lastPartOfCode) : financialDetails.Type == Usertype.Staff ? "02-002-0002-000" + Convert.ToString(lastPartOfCode) : "03-003-0003-000" + Convert.ToString(lastPartOfCode);
                }
                else
                {
                    newAcode = financialDetails.Type == Usertype.Customer ? "01-001-0001-0001": financialDetails.Type == Usertype.Staff ? "02-002-0002-0001": "03-003-0003-0001";
                }
                financialDetails.AccountNumber = newAcode;
                await dbContext.FinancialDetails.AddAsync(financialDetails);
                await dbContext.SaveChangesAsync();
                return financialDetails.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FinancialDetails>> GetFinancialDetailsAsync(string parentId)
        {
            try
            {
                List<FinancialDetails> financialDetails = await dbContext.FinancialDetails.Where(x => x.ParentId == parentId).ToListAsync();
                return financialDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FinancialDetails> GetFinancialDetailByUserIdAsync(string userId)
        {
            try
            {
                FinancialDetails financialDetails = await dbContext.FinancialDetails.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                return financialDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<FinancialDetails> GetFinancialDetailsStaffAsync(string staffId)
        {
            try
            {
                FinancialDetails financialDetails = await dbContext.FinancialDetails.Where(x => x.UserId == staffId).FirstOrDefaultAsync();
                return financialDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<PaymentInfoDto>> GetPaymentsAsync(string UserId, string role)
        {
            try
            {
                List<PaymentInfo> paymentInfo = new List<PaymentInfo>();
                if (role == "Admin")
                    paymentInfo = await dbContext.Payment.ToListAsync();
                else
                    paymentInfo = await dbContext.Payment.Where(x => x.UserId == UserId).ToListAsync();

                var users = await dbContext.User.ToListAsync();
                List<PaymentInfoDto> paymentInfoDto = mapper.Map<List<PaymentInfoDto>>(paymentInfo);
                if(paymentInfoDto !=null && paymentInfo.Count > 0 && users !=null && users.Count > 0)
                {
                    foreach(var payment in paymentInfoDto)
                    {
                        payment.UserName = users.Where(x => x.Id == payment.UserId).FirstOrDefault() !=null ? users.Where(x => x.Id == payment.UserId).FirstOrDefault().FirstName : string.Empty;
                    }
                }
                return paymentInfoDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PaymentInfoDto> GetPaymentByIdAsync(int Id)
        {
            try
            {
                PaymentInfo paymentInfo = await dbContext.Payment.Where(x => x.Id == Id).FirstOrDefaultAsync();
                PaymentInfoDto paymentInfoDto = mapper.Map<PaymentInfoDto>(paymentInfo);
                if (paymentInfoDto != null)
                {
                    var user = await dbContext.User.Where(x => x.Id == paymentInfoDto.UserId).FirstOrDefaultAsync();
                    UserDto userDto = mapper.Map<UserDto>(user);
                    paymentInfoDto.UserData = userDto;
                }
                return paymentInfoDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<FinancialDetails>> GetFinancialAccounts()
        {
            try
            {
                List<FinancialDetails> financialDetails = dbContext.FinancialDetails.ToList();
                return financialDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FinancialDetails> GetAllFinancialAccounts()
        {
            try
            {
                List<FinancialDetails> financialDetails =  dbContext.FinancialDetails.ToList();
                return financialDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> AddTransactionAsync(Transaction Transactions)
        {
            try
            {
                await dbContext.Transaction.AddAsync(Transactions);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Service>> GetSubsAsync(string UserId)
        {
            try
            {
                List<Service> services = new List<Service>();
                var SubInfo = await dbContext.UserSubcription.Where(x => x.UserId == UserId).ToListAsync();
                var getServices=await dbContext.packageService.Where(x => x.PricePlanId == SubInfo[0].PricePlanId).ToListAsync();
                foreach (var item in getServices)
                {
                    
                    var getname = dbContext.Service.Where(x => x.Id == item.ServiceId).FirstOrDefault();
                    services.Add(getname);
                }

                return services;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
