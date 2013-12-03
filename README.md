Windows 8 のスタート画面にはアプリのタイルがさまざまなサイズで表示されます。

![スタート画面](https://qiita-image-store.s3.amazonaws.com/0/32633/fa93f03c-c0ad-1eb5-6223-fd7500c08537.png)

**こんな風にストアアプリでいろんなサイズのタイルを GridView に表示したい！** と思う方も多いハズ。
この記事では GridView を拡張し、VariableSizedWrapGrid を利用してこれを実現する方法についてご紹介します。

## サンプルコード

下記の GitHub リポジトリにて公開しています。

[GitHub VariableSizedGridViewSample](https://github.com/tatsuji-kuroyanagi/VariableSizedGridViewSample)

※情報取得に Flickr API を利用していますが、API キーを指定するまではダミーのキャッシュデータを読み込むようになっています。
実際に Flickr のデータを取得するにはアプリを Flickr に登録し、コード内の FlickrConstants.cd に API キーを指定してください。

## 実装のポイント

* GridView を継承した VariableSizedGridView を作成し PrepareContainerForItemOverride メソッドを override します
* override した PrepareContainerForItemOverride メソッド内でコンテナである GridViewItem の VariableSizedWrapGrid.ColumnSpan と RowSpan の添付プロパティに ViewModel のプロパティをバインドします
* GridView の GroupStyle.Panel に VariableSizedWrapGrid を設定します
* GridView の ItemsPanel に VirtualizingStackPanel を設定します

## 具体的なやり方

まずは下記のように GridView を継承した VariableSizedGridView を作成し、PrepareContainerForItemOverride  メソッドを override します。

```VariableSizedGridView.cs
    /// <summary>
    /// 色々なサイズのタイルを表示する GridView
    /// </summary>
    public class VariableSizedGridView : GridView
    {
        /// <summary>
        /// 指定された項目を表示するために、指定された要素を準備します
        /// </summary>
        /// <param name="element">指定された項目を表示するために使用する要素</param>
        /// <param name="item">表示する項目</param>
        protected override void PrepareContainerForItemOverride(
            DependencyObject element, object item)
        {
            var container = element as FrameworkElement;

            if(container != null)
            {
                // Container に ViewModel の ColumnSpan と RowSpan をバインドする
                container.SetBinding(VariableSizedWrapGrid.ColumnSpanProperty,
                    new Binding()
                    {
                        Source = item,
                        Path = new PropertyPath("ColumnSpan"),
                        Mode = BindingMode.OneTime,
                        TargetNullValue = 1,
                        FallbackValue = 1,
                    });

                container.SetBinding(VariableSizedWrapGrid.RowSpanProperty,
                    new Binding()
                    {
                        Source = item,
                        Path = new PropertyPath("RowSpan"),
                        Mode = BindingMode.OneTime,
                        TargetNullValue = 1,
                        FallbackValue = 1,
                    });
            }

            base.PrepareContainerForItemOverride(element, item);
        }
    }
```

PrepareContainerForItemOverride の引数、element には GridViewItem、item には ViewModel が入ってくるので両者の ColumnSpan、RowSpan プロパティをバインドします。バインド失敗時や Null 値が指定された場合は 1 が設定されるようにしておきます。

次に画面側の XAML に GridView の代わりに拡張した VariableSizedGridView を配置し、GroupStyle.Panel に VariableSizedWrapGrid、ItemsPanel に VirtualizingStackPanel を設定します。
※グループ化表示しない場合は ItemsPanel に VariableSizedWrapGrid を設定するだけで大丈夫です。

```TopPage.xaml
<control:VariableSizedGridView 
    x:Name="itemGridView"
    AutomationProperties.AutomationId="ItemGridView"
    AutomationProperties.Name="Grouped Items"
    Grid.Row="1" 
    Padding="120,0,120,40"
    SizeChanged="OnSizeChanged"
    ItemsSource="{Binding Source={StaticResource groupedItemsViewSource}}"
    ItemTemplate="{StaticResource ItemTemplate}"
    IsItemClickEnabled="True"
    ItemClick="OnItemClick"
    SelectionMode="None"
    IsSwipeEnabled="false">

    <GridView.ItemsPanel>
        <ItemsPanelTemplate>
            <VirtualizingStackPanel Orientation="Horizontal" />
        </ItemsPanelTemplate>
    </GridView.ItemsPanel>
    
    <GridView.GroupStyle>
        <GroupStyle HeaderTemplate="{StaticResource GroupHeaderTemplate}">
            <GroupStyle.Panel>
                <ItemsPanelTemplate>
                    <VariableSizedWrapGrid ItemWidth="100" ItemHeight="80"/>
                </ItemsPanelTemplate>
            </GroupStyle.Panel>
        </GroupStyle>
    </GridView.GroupStyle>
    
</control:VariableSizedGridView>
```

VariableSizedWrapGrid の ItemWidth、ItemHeight にはそれぞれ最小単位のサイズを指定します。
ItemsPanel に VirtualizingStackPanel を指定するのは、下記の情報にある通り、既定の ItemsStackPanel の場合に GroupStyle の設定が無視されてしまうためです。

[Windows 8.1 での API の変更点 ー GroupStyle クラス](http://msdn.microsoft.com/ja-jp/library/windows/apps/dn263110.aspx#GroupStyle____)

> ListView と GridView の GroupStyle クラスの 3 つのプロパティは非推奨となり、無視されます。ItemsPanel が ItemsStackPanel の場合、GroupStyle の Panel、ContainerStyle、ContainerStyleSelector プロパティは優先されません。
>・・・これを回避するには、Windows 8 パネル (VSP) に ItemsPanel を設定し直し、ビジュアル ツリーの構造に関する推定を削除します。

次期バージョンで使えなくなるのか気になるところですが 8.1 までは大丈夫そうです。

あとはタイルにバインドする ViewModel に ColumnSpan、RowSpan のプロパティを追加し、表示したい大きさに合わせてそれぞれの数値を指定します。

```PhotoItemViewModel.cs
    /// <summary>
    /// 写真アイテムの ViewModel
    /// </summary>
    [DataContract]
    public class PhotoItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Column サイズ
        /// </summary>
        [DataMember]
        public int ColumnSpan
        {
            get { return this.columnSpan; }
            set { this.SetProperty(ref this.columnSpan, value); }
        }

        /// <summary>
        /// Row サイズ
        /// </summary>
        [DataMember]
        public int RowSpan
        {
            get { return this.rowSpan; }
            set { this.SetProperty(ref this.rowSpan, value); }
        }
```

必要な実装をした上で実行してみると・・・

![トップ画面](https://qiita-image-store.s3.amazonaws.com/0/32633/6fecd8c1-36d3-1093-9d6d-31ffbc7897aa.png)

無事にいろんなサイズのタイルが表示されました。
