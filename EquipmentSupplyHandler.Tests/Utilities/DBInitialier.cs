using ESHRepository.EF;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ESHRepository.Interfaces.Model;
using ESHRepository.Model;
using System.Globalization;
using System;

namespace EquipmentSupplyHandler.Tests.Utilities
{
    class DBInitializer
    {
        public static void InitializeDbForTests(ESHContext db)
        {
            db.EquipmentTypes.AddRange(GetEquipmentType());
            db.Suppliers.AddRange(GetSuppliers());
            db.Supplies.AddRange(GetDeliveries());
            db.SaveChanges();
        }

        public static List<EquipmentType> GetEquipmentType()
        {
            return new List<EquipmentType>() {
                new EquipmentType() { Id="1", Name = "Шурупповерт" },
                new EquipmentType() { Id="2", Name = "Дрель" },
                new EquipmentType() { Id="3", Name = "Бензопила" },
                new EquipmentType() { Id="4", Name = "Электролобзик" },
                new EquipmentType() { Id="5", Name = "Паяльная станция" },
                new EquipmentType() { Id="6", Name = "Насосная станция" },
            };
        }

        public static List<Supplier> GetSuppliers()
        {
            return new List<Supplier>() {
                new Supplier() { Id="1", Name = "НВС-Строй" },
                new Supplier() { Id="2", Name = "Электростройсервис" },
                new Supplier() { Id="3", Name = "Ставропольстройопторг" },
                new Supplier() { Id="4", Name = "Держава" },
                new Supplier() { Id="5", Name = "Новосел" },
            };
        }
        public static IEnumerable<Supply> GetDeliveries()
        {
            int id = 1;
            for (int year = 2016; year <= 2018; year++)
                for (int month = 1; month <= 12; month++)
                    for (int day = 1; day <= 28; day++)
                        for (int equipmentId = 1; equipmentId <= 6; equipmentId++)
                            for (int supplierId = 1; supplierId <= 5; supplierId++)
                                yield return new Supply()
                                {
                                    Id = (id++).ToString(),
                                    Count = equipmentId * supplierId,
                                    DeliveryDate = new DateTime(year, month, day),
                                    EquipmentTypeId = equipmentId.ToString(),
                                    SupplierId = supplierId.ToString(),
                                };
        }
    }
}

