// <copyright file="ViewModelBase.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>

namespace ADB2CROPCXamarinForms.ViewModels.Base
{
	using System.Runtime.Serialization;
	using System.Threading.Tasks;
	using ADB2CROPCXamarinForms.Interfaces;
	using Xamarin.Forms;
	using Xamarin.Forms.Internals;

	/// <summary>View model base class.</summary>
	[Preserve(AllMembers = true)]
	[DataContract]
	public abstract class ViewModelBase : ExtendedBindableObject
	{
		private IDialogService dialogService;

		/// <summary>View is busy.</summary>
		private bool isBusy;

		/// <summary>View title.</summary>
		private string title;

		/// <summary>Initialises a new instance of the <see cref="ViewModelBase" /> class.</summary>
		public ViewModelBase()
		{
		}

		public IDialogService DialogService => dialogService ?? (dialogService = DependencyService.Resolve<IDialogService>());

		/// <summary>Gets or sets a value indicating whether the view is busy.</summary>
		public bool IsBusy
		{
			get => isBusy;
			set
			{
				isBusy = value;
				NotifyPropertyChanged(() => IsBusy);
			}
		}

		/// <summary>gets or sets a value for the view title.</summary>
		public string Title
		{
			get => title;
			set
			{
				title = value;
				NotifyPropertyChanged(() => Title);
			}
		}

		/// <summary>Initialises the view model.</summary>
		/// <returns>Task result</returns>
		public virtual Task InitializeAsync(object parameter)
		{
			return Task.FromResult(false);
		}
	}
}