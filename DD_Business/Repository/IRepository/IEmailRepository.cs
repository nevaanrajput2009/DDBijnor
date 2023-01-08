using DD_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DD_Business.Repository.IRepository
{
    public interface IEmailRepository
    {
        void SendEmail(OrderDTO orderDetails);
        void SendOrderCheckedEmail(OrderDTO orderDetails);
    }
}
