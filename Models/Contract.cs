using System;

namespace Parking.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Amount { get; set; }
        public int ParkingLotId { get; set; }
        public ParkingLot ParkingLot { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int GuardId { get; set; }
        public Guard Guard { get; set; }


    }
}
