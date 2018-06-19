﻿using System;
using System.Runtime.Serialization;
namespace CandyShopService.BindingModels
{
    [DataContract]
    public class ReportBindingModel
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public DateTime? DateFrom { get; set; }

        [DataMember]
        public DateTime? DateTo { get; set; }
    }
}
