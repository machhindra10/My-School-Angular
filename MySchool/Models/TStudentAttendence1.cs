﻿using System;
using System.Collections.Generic;

namespace MySchool.Models
{
    public partial class TStudentAttendence1
    {
        public long Id { get; set; }
        public int? Studentid { get; set; }
        public int? Classid { get; set; }
        public int? Batchid { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public string _1 { get; set; }
        public string _2 { get; set; }
        public string _3 { get; set; }
        public string _4 { get; set; }
        public string _5 { get; set; }
        public string _6 { get; set; }
        public string _7 { get; set; }
        public string _8 { get; set; }
        public string _9 { get; set; }
        public string _10 { get; set; }
        public string _11 { get; set; }
        public string _12 { get; set; }
        public string _13 { get; set; }
        public string _14 { get; set; }
        public string _15 { get; set; }
        public string _16 { get; set; }
        public string _17 { get; set; }
        public string _18 { get; set; }
        public string _19 { get; set; }
        public string _20 { get; set; }
        public string _21 { get; set; }
        public string _22 { get; set; }
        public string _23 { get; set; }
        public string _24 { get; set; }
        public string _25 { get; set; }
        public string _26 { get; set; }
        public string _27 { get; set; }
        public string _28 { get; set; }
        public string _29 { get; set; }
        public string _30 { get; set; }
        public string _31 { get; set; }

        public Batches Batch { get; set; }
        public MClasses Class { get; set; }
        public Student Student { get; set; }
    }
}
