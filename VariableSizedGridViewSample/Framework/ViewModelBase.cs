using System.Runtime.Serialization;
using VariableSizedGridViewSample.Common;
using VariableSizedGridViewSample.Framework;

namespace VariableSizedGridViewSample.ViewModels
{
    /// <summary>
    /// ViewModel 基底クラス
    /// </summary>
    [DataContract]
    public class ViewModelBase : BindableBase, ICleanup
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        public virtual void Initilize()
        {
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Discard()
        {
            this.Cleanup();
        }

        /// <summary>
        /// インスタンスを解放します
        /// </summary>
        public virtual void Cleanup()
        {
        }
    }
}
