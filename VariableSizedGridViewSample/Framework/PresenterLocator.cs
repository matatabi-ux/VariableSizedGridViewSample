using System;
using System.Collections.Generic;
using VariableSizedGridViewSample.Presenters;
using Windows.UI.Xaml;

namespace VariableSizedGridViewSample.Framework
{
    /// <summary>
    /// Presenter 管理クラス
    /// </summary>
    public class PresenterLocator
    {
        #region Privates

        /// <summary>
        /// Presenter の登録情報
        /// </summary>
        private static readonly Dictionary<Type, Type> Registoriy = new Dictionary<Type, Type>();

        /// <summary>
        /// Presenter コンテナ
        /// </summary>
        private static readonly Dictionary<Type, IPresenter> Container = new Dictionary<Type, IPresenter>();

        #endregion //Privates

        /// <summary>
        /// 指定された Presenter を管理対象に登録する
        /// </summary>
        /// <typeparam name="TPresenter"></typeparam>
        public static void Register<TPresenter, TView>()
            where TPresenter : IPresenter
            where TView : FrameworkElement
        {
            Registoriy.Add(typeof(TView), typeof(TPresenter));
        }

        /// <summary>
        /// 指定された View の Presenter を取得する
        /// </summary>
        /// <param name="viewType">View</param>
        /// <returns>Presenter</returns>
        public static IPresenter Get(Type viewType)
        {
            if(!Container.ContainsKey(viewType))
            {
                var presenterType = Registoriy[viewType];

                Container.Add(viewType, Activator.CreateInstance(presenterType) as IPresenter);
            }

            return Container[viewType];
        }
    }
}
