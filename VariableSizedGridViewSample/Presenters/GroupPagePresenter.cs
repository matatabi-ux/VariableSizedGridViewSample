using System;
using System.Linq;
using VariableSizedGridViewSample.Common;
using VariableSizedGridViewSample.Framework;
using VariableSizedGridViewSample.ViewModels;
using VariableSizedGridViewSample.Views;
using Windows.UI.Xaml.Controls;

namespace VariableSizedGridViewSample.Presenters
{
    /// <summary>
    /// グループ詳細画面 Presenter 
    /// </summary>
    public class GroupPagePresenter : PagePresenterBase<GroupPage, GroupPageViewModel>, IGridPagePresenter
    {
        /// <summary>
        /// 画面読み込み時の処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected override void OnLoadState(object sender, Common.LoadStateEventArgs e)
        {
            base.OnLoadState(sender, e);

            var mainViewModel = ViewModelLocator.Get<MainViewModel>();
            if (e.PageState != null && e.PageState.ContainsKey("currentItem"))
            {
                mainViewModel.CurrentItem = e.PageState["currentItem"] as PhotoItemViewModel;
            }
            if (e.PageState != null && e.PageState.ContainsKey("currentGroup"))
            {
                mainViewModel.CurrentGroup = e.PageState["currentGroup"] as PhotoGroupViewModel;
            }
            var viewModel = (GroupPageViewModel)this.ViewModel;

            var group = mainViewModel.Groups.Where(g => g.UniqueId.Equals(e.NavigationParameter as string)).FirstOrDefault();

            viewModel.Header = group.Header;
            viewModel.AllItems = group.Items;

            this.View.DataContext = this.ViewModel;
        }

        /// <summary>
        /// セッション保存時の処理
        /// </summary>
        /// <param name="sender">イベント発行者</param>
        /// <param name="e">イベント引数</param>
        protected override void OnSaveState(object sender, SaveStateEventArgs e)
        {
            base.OnSaveState(sender, e);

            var mainViewModel = ViewModelLocator.Get<MainViewModel>();
            e.PageState["currentItem"] = mainViewModel.CurrentItem;
            e.PageState["currentGroup"] = mainViewModel.CurrentGroup;
        }

        #region IGridPagePresenter

        /// <summary>
        /// スクロール位置を復元する
        /// </summary>
        /// <param name="listViewBase">スクロールさせるコントロール</param>
        public void ScrollIntoView(ListViewBase listViewBase)
        {
            var mainViewModel = ViewModelLocator.Get<MainViewModel>();
            if (mainViewModel.CurrentItem != null)
            {
                listViewBase.ScrollIntoView(mainViewModel.CurrentItem);
                mainViewModel.CurrentItem = null;
            }
        }

        /// <summary>
        /// ヘッダクリック
        /// </summary>
        /// <param name="group">クリックしたグループ</param>
        public void HeaderClick(PhotoGroupViewModel group)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// アイテムクリック
        /// </summary>
        /// <param name="item">クリックしたアイテム</param>
        public void ItemClick(PhotoItemViewModel item)
        {
            ViewModelLocator.Get<MainViewModel>().CurrentItem = item;
            this.View.Frame.Navigate(typeof(DetailPage), item.UniqueId);
        }

        #endregion //IGridPagePresenter
    }
}
