using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace VariableSizedGridViewSample.ViewModels
{
    /// <summary>
    /// メイン ViewModel
    /// </summary>
    [DataContract]
    public class MainViewModel : ViewModelBase
    {
        #region Privates

        /// <summary>
        /// 情報取得中フラグ
        /// </summary>
        private bool isBusy = false;

        #endregion //Privates

        /// <summary>
        /// 情報取得中フラグ
        /// </summary>
        [IgnoreDataMember]
        public bool IsBusy
        {
            get { return this.isBusy; }
            set { this.SetProperty<bool>(ref this.isBusy, value); }
        }

        /// <summary>
        /// 写真情報
        /// </summary>
        [DataMember]
        public ObservableCollection<PhotoGroupViewModel> Groups { get; set; }

        /// <summary>
        /// 現在のアイテム
        /// </summary>
        [IgnoreDataMember]
        public PhotoItemViewModel CurrentItem { get; set; }

        /// <summary>
        /// 現在のグループ
        /// </summary>
        [IgnoreDataMember]
        public PhotoGroupViewModel CurrentGroup { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel()
        {
            this.Groups = new ObservableCollection<PhotoGroupViewModel>();
        }
    }
}
