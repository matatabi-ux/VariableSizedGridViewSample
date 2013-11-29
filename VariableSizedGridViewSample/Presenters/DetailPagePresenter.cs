using System.Linq;
using VariableSizedGridViewSample.Framework;
using VariableSizedGridViewSample.ViewModels;
using VariableSizedGridViewSample.Views;

namespace VariableSizedGridViewSample.Presenters
{
    /// <summary>
    /// アイテム詳細画面 Presenter
    /// </summary>
    public class DetailPagePresenter : PagePresenterBase<DetailPage, DetailPageViewModel>, IPagePresenter
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
            var viewModel = (DetailPageViewModel)this.ViewModel;

            var uniqueId = (string)e.NavigationParameter;

            viewModel.CurrentItem = mainViewModel.Groups
                .SelectMany<PhotoGroupViewModel, PhotoItemViewModel>(g => g.Items)
                .Where(i => uniqueId.Equals(i.UniqueId)).FirstOrDefault();

            viewModel.CurrentGroup = mainViewModel.Groups
                .Where(g => g.Items.Contains(viewModel.CurrentItem)).FirstOrDefault();

            this.View.DataContext = viewModel;
        }
    }
}
