using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PoolLab.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoolLab.Application.Services
{
    public class TableStatusUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public TableStatusUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var tableService = scope.ServiceProvider.GetRequiredService<IBilliardTableService>();
                    await tableService.UpdateBidaTableStatusAuto();
                }

                await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken); // Kiểm tra mỗi phút
            }
        }
    }
}
