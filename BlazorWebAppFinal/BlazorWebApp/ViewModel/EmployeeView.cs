﻿// ***********************************************************************
// Assembly         : SimpleNonIndexList
// Author           : James Thompson
// Created          : 11-30-2022
//
// Last Modified By : James Thompson
// Last Modified On : 11-30-2022
// ***********************************************************************
// <copyright file="EmployeeView.cs" company="SimpleNonIndexList">
//     Copyright (c) NAIT. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace BlazorWebApp.ViewModel
{
    /// <summary>
    /// Class Employee.
    /// </summary>
    public class EmployeeView
    {
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// <value>The employee identifier.</value>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
    }
}
