
/* TODO */
// 渦巻き状にループするfor文を作る <= 完了
// ブレゼンハムのアルゴリズム <= 完了
// 畳み込みを使用した価値の判定
// フロッキングアルゴリズムの続き <= 完了

// 学んだ
// オブジェクト初期化子によるコンストラクタを使用しない初期化
// ヘルパークラス(オブジェクト思考的ユーティリティクラス)
// StringBuilderクラス…文字列を合体させるために使う
// ★街の自動生成について勉強になった
//  重要:アルゴリズムにオブジェクト指向的なコーディングはむりぽ
//  今まではどこに置くかしか制御できなかったが、
//  どの向きに置くか、置いた後に制御できるかまで考慮する必要がある。
//  各道路がどの方向に接続されているか知ることが出来る状態にしておく。
//  建物ごとではなく、同じパラメータの建物を一括で管理する。
//  ICollectionインターフェースを引数にすることでリスト以外のコレクションを渡すことが出来る。
// 角度から方向ベクトルを求める事でCenterPos + dir で
//  dir = Quaternion.AngleAxis(angle, Vector3.up) * dir;
// Switch文のCaseはbreakではなくreturnで抜けることも出来る。
// インスペクターからGameObjectのプレハブをわり会える時の名前 _prefab とする
//  なんのプレハブを割りあてれば良いのかをHeader属性で書いておくとわかりやすい
// ★VSショートカット
// 対象にカーソルが合っている状態で ctrl + . で型とかクイックアクションを呼び出せる
// Ctrl + Alt + PageUp/PageDown
// ★ダンジョン生成の考え方
//  Vector2Int型のリストやハッシュセットに座標を格納&UnionWith()メソッドで合体させる