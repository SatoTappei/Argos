using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class LSystemGenerator : MonoBehaviour
{
    // 再帰的に道路を伸ばす角度を90度にすることで街っぽくなる
    // 文字の置き換えのルールを適用するを繰り返すAlgorithm
    // フラクタル構造(似たような形が繰り返される)である。
    // [F]--F
    // l[+F][-F] Save,右を向く,前に進む,戻る,セーブ,左を向く,前に進む,戻る
    // […Save ]…Load 座標と回転を保存

    [SerializeField] Rule[] _rules;
    [SerializeField] string _rootSentence;
    [Range(0, 10)]
    [SerializeField] int _iterationLimit = 1;
    [SerializeField] bool _randomIgnore = true;
    [Range(0, 1)]
    [SerializeField] float _chanceToIgnore = 0.3f;

    void Start()
    {
        // Sentence生成メソッドを呼んで結果をログに表示
        Debug.Log(GenerateSentence());
    }

    // 引数の文字からセンテンスを生成して返す
    public string GenerateSentence(string word = null)
    {
        if (word == null)
        {
            word = _rootSentence;
        }
        // 引数の文字を再帰的に成長させる
        return GrowRecursive(word);
    }

    /// <summary>引数の文字を再帰的に成長させるメソッド</summary>
    string GrowRecursive(string word, int iterationIndex = 0)
    {
        // 制限を超えたらこれ以上成長させないで文字列を返す
        if (iterationIndex >= _iterationLimit)
        {
            return word;
        }

        // 引数の文字列に対して再帰的な処理
        // 下のforeachの中身が再帰的に処理される
        StringBuilder builder = new StringBuilder();
        foreach (char c in word)
        {
            // 文字列ビルダーに追加してルールを適用して処理する
            builder.Append(c);
            ProcessRulesRecursivelly(builder, c, iterationIndex);
        }

        // 再帰処理後に文字列にして返す
        return builder.ToString();
    }

    /// <summary>
    /// 再帰処理をする
    /// </summary>
    /// <param name="builder">文字列ビルダーを渡す(依存性の注入)</param>
    /// <param name="c"></param>
    /// <param name="iterationIndex"></param>
    void ProcessRulesRecursivelly(StringBuilder builder, char c, int iterationIndex)
    {
        // 全ルールを適用する
        foreach (Rule rule in _rules)
        {
            // 引数の文字がルール適用文字と一致していたら
            if (rule.Letter == c.ToString())
            {
                // ランダム性を無視するフラグが立っている場合
                // 繰り返す回数が1以上の場合
                // 確率でこれ以降のエッジの生成がされない
                if (_randomIgnore && iterationIndex > 1)
                {
                    if (Random.value < _chanceToIgnore)
                    {
                        return;
                    }
                }

                // 文字列に追加する(再帰処理)
                // ルールによって置き換えた文字列、添え字番号+1
                builder.Append(GrowRecursive(rule.GetResult(), iterationIndex + 1));
            }
        }
    }
}
