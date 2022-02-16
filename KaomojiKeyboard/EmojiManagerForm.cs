using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace KaomojiKeyboard
{
    public partial class EmojiManagerForm : Form
    {
        public EmojiManagerForm()
        {
            InitializeComponent();
        }

        Dictionary<string, object> Settings = new Dictionary<string, object>();
        Dictionary<string, string[]> EmojiData = new Dictionary<string, string[]>()
        {
            {"Happiness", new string[]{"(* ^ ω ^)", "(´ ∀ `*)", "٩ (◕‿◕｡) ۶", "☆ *: .｡. o (≧ ▽ ≦) o .｡.: * ☆", "(o ^ ▽ ^ o)", "(⌒ ▽ ⌒) ☆", "<(￣︶￣)>", "。.: ☆ *: ･ ‘(* ⌒ ― ⌒ *)))", "ヽ (・ ∀ ・) ﾉ", "(´｡ • ω • ｡`)", "(￣ω￣)", "｀;: ゛; ｀; ･ (° ε °)", "(o ･ ω ･ o)", "(＠ ＾ ◡ ＾)", "ヽ (* ・ ω ・) ﾉ", "(o_ _) ﾉ 彡 ☆", "(^ 人 ^)", "(o´ ▽ `o)", "(* ´ ▽ `*)", "｡ ﾟ (ﾟ ^ ∀ ^ ﾟ) ﾟ｡", "(´ ω `)", "(((o (* ° ▽ ° *) o)))", "(≧ ◡ ≦)", "(o´∀`o)", "(´ • ω • `)", "(＾ ▽ ＾)", "(⌒ω⌒)", "∑d (° ∀ ° d)", "╰ (▔∀▔) ╯", "(─‿‿─)", "(* ^ ‿ ^ *)", "ヽ (o ^ ^ o) ﾉ", "(✯◡✯)", "(◕‿◕)", "(* ≧ ω ≦ *)", "(☆ ▽ ☆)", "(⌒‿⌒)", "＼ (≧ ▽ ≦) ／", "ヽ (o ＾ ▽ ＾ o) ノ", "☆ ～ (‘▽ ^ 人)", "(* ° ▽ ° *)", "٩ (｡ • ́‿ • ̀｡) ۶", "(✧ω✧)", "ヽ (* ⌒ ▽ ⌒ *) ﾉ", "(´｡ • ᵕ • ｡`)", "(´ ▽ `)", "(￣ ▽ ￣)", "╰ (* ´︶` *) ╯", "ヽ (> ∀ <☆) ノ", "o (≧ ▽ ≦) o", "(☆ ω ☆)", "(っ ˘ω˘ς)", "＼ (￣ ▽ ￣) ／", "(* ¯︶¯ *)", "＼ (＾ ▽ ＾) ／", "٩ (◕‿◕) ۶", "(o˘◡˘o)", "\\ (★ ω ★) /", "\\ (^ ヮ ^) /", "(〃 ＾ ▽ ＾ 〃)", "(╯✧ ▽ ✧) ╯", "o (> ω <) o", "o (❛ᴗ❛) o", "｡ ﾟ (T ヮ T) ﾟ｡", "(‾́ ◡ ‾́)", "(ﾉ ´ ヮ `) ﾉ *: ･ ﾟ", "(b ᵔ ▽ ᵔ) b", "(๑˃ᴗ˂) ﻭ", "(๑˘︶˘๑)", "(˙꒳ ˙)", "(* ꒦ ິ ꒳꒦ ີ)", "° ˖✧◝ (⁰▿⁰) ◜✧˖ °", "(´ ･ ᴗ ･ `)", "(ﾉ ◕ ヮ ◕) ﾉ *: ･ ﾟ ✧", "(„• – •„)", "(.❛ ᴗ ❛.)", "(⁀ᗢ⁀)", "(￢‿￢)", "(¬‿¬)", "(* ￣ ▽ ￣) b", "(˙▿˙)", "(¯▿¯)"}},
            {"Love and Care", new string[]{"(ﾉ ´ з `) ノ", "(♡ μ_μ)", "(* ^^ *) ♡", "☆ ⌒ ヽ (* ‘､ ^ *) chu", "(♡ -_- ♡)", "(￣ε￣ ＠)", "ヽ (♡ ‿ ♡) ノ", "(´ ∀ `) ノ ～ ♡", "(─‿‿─) ♡", "(´｡ • ᵕ • ｡`) ♡", "(* ♡ ∀ ♡)", "(｡ ・ // ε // ・｡)", "(´ ω `♡)", "♡ (◡‿◡)", "(◕‿◕) ♡", "(/▽＼*)｡o○♡", "(ღ˘⌣˘ღ)", "(♡ ° ▽ ° ♡)", "♡ (｡- ω -)", "♡ ～ (‘▽ ^ 人)", "(´ • ω • `) ♡", "(´ ε `) ♡", "(´｡ • ω • ｡`) ♡", "(´ ▽ `) .｡ ｏ ♡", "╰ (* ´︶` *) ╯ ♡", "(* ˘︶˘ *) .｡.: * ♡", "(♡ ˙︶˙ ♡)", "♡ ＼ (￣ ▽ ￣) ／ ♡", "(≧ ◡ ≦) ♡", "(⌒ ▽ ⌒) ♡", "(* ¯ ³¯ *) ♡", "(っ ˘з (˘⌣˘) ♡", "♡ (˘ ▽ ˘> ԅ (˘⌣˘)", "(˘⌣˘) ♡ (˘⌣˘)", "(/ ^ – ^ (^ ^ *) / ♡", "٩ (♡ ε ♡) ۶", "σ (≧ ε ≦ σ) ♡", "♡ (⇀ 3 ↼)", "♡ (￣З￣)", "(❤ω❤)", "(˘∀˘) / (μ‿μ) ❤", "❤ (ɔˆз (ˆ⌣ˆc)", "(´ ♡ ‿ ♡ `)", "(° ◡ ° ♡)", "Σ> – (〃 ° ω ° 〃) ♡ →", "(´ ,, • ω • ,,) ♡"}},
            {"Embarassment", new string[]{"(⌒_⌒;)", "(o ^ ^ o)", "(* / ω＼)", "(* /。 ＼)", "(* / _ ＼)", "(* ﾉ ω ﾉ)", "(o -_- o)", "(* μ_μ)", "(◡‿◡ *)", "(ᵔ.ᵔ)", "(* ﾉ ∀` *)", "(// ▽ //)", "(// ω //)", "(ノ * ° ▽ ° *)", "(* ^. ^ *)", "(* ﾉ ▽ ﾉ)", "(￣ ▽ ￣ *) ゞ", "(⁄ ⁄ • ⁄ω⁄ • ⁄ ⁄)", "(* / ▽ ＼ *)", "(⁄ ⁄> ⁄ ▽ ⁄ <⁄ ⁄)", "(„ಡ ω ಡ„)", "(ง ื ▿ ื) ว", "(〃 ▽ 〃)"}},
            {"Sympathy", new string[]{"(ノ _ <。) ヾ (´ ▽ `)", "｡ ･ ﾟ ･ (ﾉ Д`) ヽ (￣ω￣)", "ρ (- ω – 、) ヾ (￣ω￣;)", "ヽ (￣ω￣ (。。) ゝ", "(* ´ Tôi `) ﾉ ﾟ (ﾉ Д ｀ ﾟ) ﾟ｡", "ヽ (~ _ ~ (・ _ ・) ゝ", "(ﾉ _ ；) ヾ (´ ∀ `)", "(; ω;) ヾ (´∀` *)", "(* ´ ー) ﾉ (ノ д`)", "(´-ω-`(_ _)", "(っ ´ω`) ﾉ (╥ω╥)", "(ｏ ・ _ ・) ノ ”(ノ _ <、)"}},
            {"Disagreement", new string[]{"(＃ ＞ ＜)", "(； ⌣̀_⌣́)", "☆ ｏ (＞ ＜ ；) ○", "(￣ ￣ |||)", "(； ￣Д￣)", "(￣ □ ￣ 」)", "(＃ ￣0￣)", "(＃ ￣ω￣)", "(￢_￢;)", "(＞ ｍ ＜)", "(」° ロ °)」", "(〃 ＞ ＿ ＜; 〃)", "(＾＾ ＃)", "(︶︹︺)", "(￣ ヘ ￣)", "<(￣ ﹌ ￣)>", "(￣︿￣)", "(＞ ﹏ ＜)", "(–_–)", "凸 (￣ ヘ ￣)", "ヾ (￣O￣) ツ", "(⇀‸↼ ‶)", "o (> <) o", "(」＞ ＜)」", "(ᗒᗣᗕ)՞", "(눈 _ 눈)"}},
            {"Anger", new string[]{"(＃ `Д´)", "(`皿 ´ ＃)", "(`ω ´)", "ヽ (`д´ *) ノ", "(・ `Ω´ ・)", "(`ー ´)", "ヽ (`⌒´ メ) ノ", "凸 (`△ ´ ＃)", "(`ε´)", "ψ (`∇ ´) ψ", "ヾ (`ヘ ´) ﾉ ﾞ", "ヽ (‵ ﹏´) ノ", "(ﾒ `ﾛ ´)", "(╬` 益 ´)", "┌∩┐ (◣_◢) ┌∩┐", "凸 (`ﾛ ´) 凸", "Σ (▼ □ ▼ メ)", "(° ㅂ ° ╬)", "ψ (▼ へ ▼ メ) ～ →", "(ノ ° 益 °) ノ", "(҂ `з´)", "(‡ ▼ 益 ▼)", "(҂` ﾛ ´) 凸", "((╬◣﹏◢))", "٩ (╬ʘ 益 ʘ╬) ۶", "(╬ Ò﹏Ó)", "＼＼٩ (๑` ^ ´๑) ۶ ／／", "(凸 ಠ 益 ಠ) 凸", "↑ _ (ΦwΦ) Ψ", "← ~ (Ψ ▼ ｰ ▼) ∈", "୧ ((# Φ 益 Φ #)) ୨", "٩ (ఠ 益 ఠ) ۶", "(ﾉ ಥ 益 ಥ) ﾉ"}},
            {"Sadness", new string[]{"(ノ _ <。)", "(-_-)", "(´-ω-`)", ". ･ ﾟ ﾟ ･ (／ ω＼) ･ ﾟ ﾟ ･.", "(μ_μ)", "(ﾉ Д`)", "(-ω- 、)", "。 ゜ ゜ (´Ｏ`) ゜ ゜。", "o (T ヘ T)", "(; ω;)", "(｡╯︵╰｡)", "｡ ･ ﾟ ﾟ * (> д <) * ﾟ ﾟ ･｡", "(ﾟ ， _ ゝ ｀)", "(个 _ 个)", "(╯︵╰,)", "｡ ･ ﾟ (ﾟ> <ﾟ) ﾟ ･｡", "(╥ω╥)", "(╯_╰)", "(╥_╥)", ".｡ ･ ﾟ ﾟ ･ (＞ _ ＜) ･ ﾟ ﾟ ･｡.", "(／ ˍ ・ 、)", "(ノ _ <、)", "(╥﹏╥)", "｡ ﾟ (｡ ﾉ ω ヽ｡) ﾟ｡", "(つ ω`｡)", "(｡T ω T｡)", "(ﾉ ω ･ ､)", "･ ﾟ ･ (｡> ω <｡) ･ ﾟ ･", "(T_T)", "(> _ <)", "(っ ˘̩╭╮˘̩) っ", "｡ ﾟ ･ (> ﹏ <) ･ ﾟ｡", "o (〒﹏〒) o", "(｡ • ́︿ • ̀｡)", "(ಥ﹏ಥ)"}},
            {"Pain", new string[]{"~ (> _ <~)", "☆ ⌒ (> _ <)", "☆ ⌒ (>。 <)", "(☆ _ @)", "(× _ ×)", "(x_x)", "(× _ ×) ⌒ ☆", "(x_x) ⌒ ☆", "(× ﹏ ×)", "☆ (＃ ××)", "(＋ _ ＋)", "[± _ ±]", "٩ (× ×) ۶", "_ 🙁 ´ ཀ `」 ∠): _"}},
            {"Fear", new string[]{"(ノ ω ヽ)", "(／。＼)", "(ﾉ _ ヽ)", ".. ・ ヾ (。 ＞ ＜) シ", "(″ ロ ゛)", "(;;; * _ *)", "(・ 人 ・)", "＼ (〇_ｏ) ／", "(/ ω＼)", "(/ _＼)", "〜 (＞ ＜) 〜", "Σ (° △ ° |||) ︴", "(((＞ ＜)))", "{{(> _ <)}}", "＼ (º □ º l | l) /", "〣 (ºΔº) 〣", "▓▒░ (° ◡ °) ░▒▓"}},
            {"Shrug", new string[]{"ヽ (ー _ ー) ノ", "ヽ (´ ー `) ┌", "┐ (‘～ `) ┌", "ヽ (￣д￣) ノ", "┐ (￣ ヘ ￣) ┌", "ヽ (￣ ～ ￣) ノ", "╮ (￣_￣) ╭", "ヽ (ˇ ヘ ˇ) ノ", "┐ (￣ ～ ￣) ┌", "┐ (︶ ▽ ︶) ┌", "╮ (￣ ～ ￣) ╭", "¯ \\ _ (ツ) _ / ¯", "┐ (´ д `) ┌", "╮ (︶︿︶) ╭", "┐ (￣∀￣) ┌", "┐ (˘ ､ ˘) ┌", "╮ (︶ ▽ ︶) ╭", "╮ (˘ ､ ˘) ╭", "┐ (˘_˘) ┌", "╮ (˘_˘) ╭", "┐ (￣ ヮ ￣) ┌", "ᕕ (ᐛ) ᕗ"}},
            {"Confusion", new string[]{"(￣ω￣;)", "σ (￣ 、 ￣〃)", "(￣ ～ ￣;)", "(-_-;) ・ ・ ・", "┐ (‘～ `;) ┌", "(・ _ ・ ヾ", "(〃￣ω￣〃 ゞ", "┐ (￣ ヘ ￣;) ┌", "(・ _ ・;)", "(￣_￣) ・ ・ ・", "╮ (￣ω￣;) ╭", "(¯. ¯;)", "(＠ _ ＠)", "(・ ・;) ゞ", "Σ (￣。￣ ﾉ)", "(・ ・)?", "(• ิ _ • ิ)?", "(◎ ◎) ゞ", "(ー ー;)", "ლ (ಠ_ಠ ლ)", "ლ (¯ ロ ¯ “ლ)", "(¯. ¯٥)", "(¯ ¯٥)"}},
            {"Suspicion", new string[]{"(￢_￢)", "(→ _ →)", "(￢ ￢)", "(￢‿￢)", "(¬_¬)", "(← _ ←)", "(¬ ¬)", "(¬‿¬)", "(↼_↼)", "(⇀_⇀)"}},
            {"Surprise", new string[]{"w (° ｏ °) w", "ヽ (° 〇 °) ﾉ", "Σ (O_O)", "Σ (° ロ °)", "(⊙_⊙)", "(o_O)", "(O_O;)", "(OO)", "(° ロ °)!", "(o_O)!", "(□ _ □)", "Σ (□ _ □)", "∑ (O_O;)", "(: ౦ ‸ ౦:)"}},
            {"Greeting", new string[]{"(* ・ Ω ・) ﾉ", "(￣ ▽ ￣) ノ", "(° ▽ °) /", "(´ ∀ `) ﾉ", "(^ – ^ *) /", "(＠ ´ ー `) ﾉ ﾞ", "(´ • ω • `) ﾉ", "(° ∀ °) ﾉ ﾞ", "ヾ (* ‘▽’ *)", "＼ (⌒ ▽ ⌒)", "ヾ (☆ ▽ ☆)", "(´ ▽ `) ﾉ", "(^ ０ ^) ノ", "~ ヾ (・ ω ・)", "(・ ∀ ・) ノ", "ヾ (・ ω ・ *)", "(* ° ｰ °) ﾉ", "(・ _ ・) ノ", "(o´ω`o) ﾉ", "(´ ▽ `) /", "(￣ω￣) /", "(´ ω `) ノ ﾞ", "(⌒ω⌒) ﾉ", "(o ^ ^ o) /", "(≧ ▽ ≦) /", "(✧∀✧) /", "(o´ ▽ `o) ﾉ", "(￣ ▽ ￣) /"}},
            {"Hug", new string[]{"(づ ￣ ³￣) づ", "(つ ≧ ▽ ≦) つ", "(つ ✧ω✧) つ", "(づ ◕‿◕) づ", "(⊃｡ • ́‿ • ̀｡) ⊃", "(つ. • ́ _ʖ • ̀.) つ", "(っ ಠ‿ಠ) っ", "(づ ◡﹏◡) づ", "⊂ (´ • ω • `⊂)", "⊂ (･ ω ･ * ⊂)", "⊂ (￣ ▽ ￣) ⊃", "⊂ (´ ▽ `) ⊃", "(~ * – *) ~"}},
            {"Wink", new string[]{"(^ _ ~)", "(ﾟ ｏ⌒)", "(^ _-) ≡ ☆", "(^ ω ~)", "(> ω ^)", "(~ 人 ^)", "(^ _-)", "(-_ ・)", "(^ _ <) 〜 ☆", "(^ 人 <) 〜 ☆", "☆ ⌒ (≧ ▽ °)", "☆ ⌒ (ゝ 。∂)", "(^ _ <)", "(^ _−) ☆", "(･ Ω <) ☆", "(^. ~) ☆", "(^. ~)"}},
            {"Apology", new string[]{"m (_ _) m", "(シ _ _) シ", "m (.) m", "<(_ _)>", "人 (_ _ *)", "(* _ _) 人", "m (_ _; m)", "(m; _ _) m", "(シ..) シ"}},
            {"Nose-bleeding", new string[]{"(* ￣ii￣)", "(￣ ﾊ ￣ *)", "\\ (￣ ﾊ ￣)", "(＾ ་ ། ＾)", "(＾ 〃 ＾)", "(￣ ¨ ヽ ￣)", "(￣; ￣)", "(￣ ;; ￣)"}},
            {"Hiding", new string[]{"| ･ Ω ･)", "ﾍ (･ _ |", "| ω ･) ﾉ", "ヾ (･ |", "| д ･)", "| _￣))", "| ▽ //)", "┬┴┬┴┤ (･ _├┬┴┬┴", "┬┴┬┴┤ ･ ω ･) ﾉ", "┬┴┬┴┤ (͡ ° ͜ʖ├┬┴┬┴", "┬┴┬┴┤ (･ _├┬┴┬┴", "| _ ・)", "| ･ Д ･) ﾉ", "| ʘ‿ʘ) ╯"}},
            {"Writing", new string[]{"__φ (．．)", "(￣ ー ￣) φ__", "__φ (。。)", "__φ (．．;)", "ヾ (`ー ´) シ φ__", "__〆 (￣ ー ￣)", "…. φ (・ ∀ ・ *)", "___ 〆 (・ ∀ ・)", "(^ ▽ ^) ψ__", "…. φ (︶ ▽ ︶) φ ….", "(..) φ__", "__φ (◎◎ ヘ)"}},
            {"Running", new string[]{"☆ ﾐ (o * ･ ω ･) ﾉ", "C = C = C = C = C = ┌ (; ・ ω ・) ┘", "─ = ≡Σ (((＞ ＜) つ", "ε = ε = ε = ε = ┌ (; ￣ ▽ ￣) ┘", "ε = ε = ┌ (> _ <) ┘", "C = C = C = C = ┌ (`ー ´) ┘", "ε === (っ ≧ ω ≦) っ", "ヽ (￣д￣;) ノ = 3 = 3 = 3", "。。。 ミ ヽ (。 ＞ ＜) ノ"}},
            {"Sleeping", new string[]{"[(－－)] .. zzZ", "(－_－) zzZ", "(∪｡∪) ｡｡｡ zzZ", "(－Ω－) zzZ", "(￣o￣) zzZZzzZZ", "((_ _)) .. zzzZZ", "(￣ρ￣) .. zzZZ", "(－.－) … zzz", "(＿ ＿ *) Z zz", "(x. x) ~~ zzZ"}},
            {"Cat", new string[]{"(= ^ ･ Ω ･ ^ =)", "(= ^ ･ ｪ ･ ^ =)", "(= ①ω① =)", "(= ω =) .. meo meo", "(=; ｪ; =)", "(= `ω´ =)", "(= ^ ‥ ^ =)", "(= ノ ω ヽ =)", "(= ⌒‿‿⌒ =)", "(= ^ ◡ ^ =)", "(= ^ – ω – ^ =)", "ヾ (= `ω´ =) ノ”", "(＾ • ω • ＾)", "(/ = ω =) /", "ฅ (• ㅅ • ❀) ฅ", "ฅ (• ɪ •) ฅ", "ଲ (ⓛ ω ⓛ) ଲ", "(^ = ◕ᴥ◕ = ^)", "(= ω =)", "ଲ (ⓛ ω ⓛ) ଲ", "(^ = ◕ᴥ◕ = ^)", "(= ω =)", "(^ ˵◕ω◕˵ ^)", "(^ ◔ᴥ◔ ^)", "(^ ◕ᴥ◕ ^)", "ต (= ω =) ต", "(Φ ω Φ)"}},
            {"Bear", new string[]{"(´ (ｴ) ˋ)", "(* ￣ (ｴ) ￣ *)", "ヽ (￣ (ｴ) ￣) ﾉ", "(／ ￣ (ｴ) ￣) ／", "(￣ (ｴ) ￣)", "ヽ (ˋ (ｴ) ´) ﾉ", "⊂ (￣ (ｴ) ￣) ⊃", "(／ (ｴ) ＼)", "⊂ (´ (ェ) ˋ) ⊃", "(/ – (ｴ) -＼)", "(/ ° (ｴ) °) /", "ʕ ᵔᴥᵔ ʔ", "ʕ • ᴥ • ʔ", "ʕ • ̀ ω • ́ ʔ", "ʕ • ̀ o • ́ ʔ"}},
            {"Dog", new string[]{"∪ ＾ ェ ＾ ∪", "∪ ･ ω ･ ∪", "∪￣-￣∪", "∪ ･ ｪ ･ ∪", "Ｕ ^ 皿 ^ Ｕ", "ＵＴ ｪ ＴＵ", "Ư ^ ｪ ^ Ư", "V ● ᴥ ● V"}},
            {"Rabbit", new string[]{"／ (≧ x ≦) ＼", "／ (･ × ･) ＼", "／ (= ´x` =) ＼", "／ (^ X ^) ＼", "／ (= ･ X ･ =) ＼", "／ (^ × ^) ＼", "／ (＞ × ＜) ＼", "／ (˃ ᆺ ˂) ＼"}},
            {"Pig", new string[]{"(´ (00) ˋ)", "(￣ (ω) ￣)", "ヽ (ˋ (00) ´) ノ", "(´ (oo) ˋ)", "＼ (￣ (oo) ￣) ／", "｡ ﾟ (ﾟ ´ (00) `ﾟ) ﾟ｡", "(￣ (00) ￣)", "(ˆ (oo) ˆ)"}},
            {"Bird", new string[]{"(￣Θ￣)", "(`･ Θ ･ ´)", "(ˋ Θ ´)", "(◉Θ◉)", "＼ (ˋ Θ ´) ／", "(･ Θ ･)", "(・ Θ ・)", "ヾ (￣ ◇ ￣) ノ 〃", "(･ Θ ･)"}},
            {"Fish", new string[]{"(°) #)) <<", "<・)))> <<", "ζ °))) 彡", "> °)))) 彡", "(°)) <<", "> ^))) <～～", "≧ (° °) ≦"}},
            {"Spider", new string[]{"/ ╲ / \\ ╭ (ఠఠ 益 ఠఠ) ╮ / \\ ╱ \\","/ ╲ / \\ ╭ (ರರ⌓ರರ) ╮ / \\ ╱ \\","/ ╲ / \\ ╭ ༼ ºº ل͟ ºº ༽ ╮ / \\ ╱ \\","/ ╲ / \\ ╭ (͡ ° ͡ ° ͜ʖ ͡ ° ͡ °) ╮ / \\ ╱ \\","/ ╲ / \\ ╭ [ᴼᴼ ౪ ᴼᴼ] ╮ / \\ ╱ \\","/ ╲ / \\ (• ̀ ω • ́) / \\ ╱ \\","/ ╲ / \\ ╭ [☉﹏☉] ╮ / \\ ╱ \\"}},
            {"Friendship", new string[]{"ヾ (・ ω ・) メ (・ ω ・) ノ","ヽ (∀ °) 人 (° ∀) ノ","ヽ (⌒o⌒) 人 (⌒-⌒) ﾉ","(* ^ ω ^) 八 (⌒ ▽ ⌒) 八 (-‿‿-) ヽ","＼ (＾ ∀ ＾) メ (＾ ∀ ＾) ノ","ヾ (￣ ー ￣ (≧ ω ≦ *) ゝ","ヽ (⌒ω⌒) 人 (= ^ ‥ ^ =) ﾉ","ヽ (≧ ◡ ≦) 八 (o ^ ^ o) ノ","(* ・ ∀ ・) 爻 (・ ∀ ・ *)","｡ *: ☆ (・ ω ・ 人 ・ ω ・) ｡: ゜ ☆｡","o (^^ o) (o ^^ o) (o ^^ o) (o ^^) o","(((￣ (￣ (￣ ▽ ￣) ￣) ￣)))","(° (° ω (° ω ° (☆ ω ☆) ° ω °) ω °) °)","ヾ (・ ω ・ `) ノ ヾ (´ ・ ω ・) ノ ゛","Ψ (`∀) (∀´) Ψ","(っ ˘ ▽ ˘) (˘ ▽ ˘) ˘ ▽ ˘ς)","(((* ° ▽ ° *) 八 (* ° ▽ ° *)))","☆ ヾ (* ´ ・ ∀ ・) ﾉ ヾ (・ ∀ ・ `*) ﾉ ☆","(* ＾ ω ＾) 人 (＾ ω ＾ *)","٩ (๑ ･ ิ ᴗ ･ ิ) ۶٩ (･ ิ ᴗ ･ ิ ๑) ۶","(☞ ° ヮ °) ☞ ☜ (° ヮ ° ☜)","＼ (▽ ￣ \\ (￣ ▽ ￣) / ￣ ▽) ／","\\ (˙▿˙) / \\ (˙▿˙) /"}},
            {"Enemy", new string[]{"ヽ (･ ∀ ･) ﾉ _θ 彡 ☆ Σ (ノ `Д´) ノ","(* ´∇`) ┌θ ☆ (ﾉ> _ <) ﾉ","(￣ω￣) ノ ﾞ ⌒ ☆ ﾐ (o _ _) o","(* `0´) θ ☆ (メ ° 皿 °) ﾉ","(o¬‿¬o) … ☆ ﾐ (* x_x)","(╬￣ 皿 ￣) = ○ ＃ (￣ #) ３￣)","(; -_-) ―――――― C <―_-)","＜ (￣︿￣) ︵θ︵θ︵ ☆ (＞ 口 ＜ -)","(￣ε (# ￣) ☆ ╰╮o (￣ ▽ ￣ ///)","ヽ (> _ <ヽ) ―⊂ | = 0 ヘ (^ ‿ ^)","ヘ (> _ <ヘ) ￢o (￣‿￣ ﾒ)",",, (((￣ □) _ ／ ＼_ (○ ￣))) ,,","(҂` ﾛ ´) ︻ デ Feat 一 ＼ (º □ º l | l) /","(╯ ° Д °) ╯︵ /(.□. ＼)","(¬_¬ ”) ԅ (￣ε￣ԅ)","/ (. □.) ＼ ︵╰ (° 益 °) ╯︵ /(.□. /)","(ﾉ -.-) ﾉ…. (((((((((((● ~ * (> _ <)","!! (ﾒ ￣ ￣) _θ ☆ ° 0 °) /","(`⌒ *) O- (` ⌒´Q)","(((ง ‘ω’) و 三 ง ‘ω’) ڡ≡ ☆ ⌒ ﾐ ((x_x)","(ง ಠ_ಠ) ง σ (• ̀ ω • ́ σ)","(っ • ﹏ •) っ ✴ == ≡ 눈 ٩ (`皿 ´҂) ง","(｢• ω •)｢ (⌒ω⌒`)","(° ᴗ °) ~ ð (/ ❛o❛ \\)"}},
            {"Weapons", new string[]{"(・ ∀ ・) ・ ・ ・ ——– ☆","(/ -_ ・) / D ・ ・ ・ ・ ・ —— →","(^ ω ^) ノ ﾞ (((((((((● ～ *","(-ω -) ／ 占 ~~~~~","(/ ・ ・) ノ ((く ((へ","―⊂ | = 0 ヘ (^^)","○ ∞∞∞∞ ヽ (^ ー ^)","(; ・ _ ・) ―――― C","(ಠ o ಠ) ¤ = [] :::::>","(* ＾＾) / ~~~~~~~~~~ ◎","￢o (￣-￣ ﾒ)","– (T_T) →","(((￣ □) _ ／","(ﾒ `ﾛ ´) ︻ デ","(´-ω ･) ︻┻┳ ━ ━","(ﾒ ￣ ▽ ￣) ︻┳ 一","✴ == ≡ 눈 ٩ (`皿 ´҂) ง","Q (`⌒´Q)"}},
            {"Magic", new string[]{"(ノ ˘_˘) ノ ζ ||| ζ ζ ||| ζ ζ ||| ζ","(ﾉ ≧ ∀ ≦) ﾉ ‥… ━━━ ★","(ﾉ> ω <) ﾉ:｡ ･: *: ･ ﾟ ‘★ ,｡ ･: *: ･ ﾟ’ ☆","(ノ ° ∀ °) ノ ⌒ ･ *: .｡. .｡.: * ･ ゜ ﾟ ･ * ☆","╰ (͡ ° ͜ʖ ͡ °) つ ── ☆ *: ・ ﾟ","(＃ ￣ □ ￣) o━∈ ・ ・ ━━━━ ☆","(⊃｡ • ́‿ • ̀｡) ⊃━✿✿✿✿✿✿","(∩ ᄑ _ ᄑ) ⊃━ ☆ ﾟ * ･｡ * ･: ≡ (ε 🙂","(/ ￣ ー ￣) / ~~ ☆ ‘. ･. ･: ★’. ･. ･: ☆","(∩` ﾛ ´) ⊃━ 炎炎 炎炎 炎"}},
            {"Foods", new string[]{"(っ ˘ڡ˘ ς)","(o˘◡˘o) ┌iii┐","(‘ω’) 旦 ~~","(˘ ▽ ˘) っ ♨","♨o (> _ <) o♨","(・ Ω ・) o – {{[〃]}}","(・ Ω ・) ⊃- [二 二]","(・ ・) つ – {} @ {} @ {} –","(・ ・) つ – ●●●","(* ´ ー `) 旦 旦 (￣ω￣ *)","(* ´з`) 口 ﾟ｡ ﾟ 口 (・ ∀ ・)","(o ^ ^ o) 且 且 (´ω` *)","(￣ ▽ ￣) [] [] (≧ ▽ ≦)","(* ^^) o∀ * ∀o (^^ *)","(^^) _ 旦 ~~ ~~ Ư _ (^^)","(* ￣ ▽ ￣) 旦 且 (´∀` *)","– ●●● -ｃ (・ ・)","(・ ・) つ – ● ○ ◎ –"}},
            {"Music", new string[]{"ヾ (´〇`) ﾉ ♪♪♪","ヘ (￣ω￣ ヘ)","(〜￣ ▽ ￣) 〜","〜 (￣ ▽ ￣〜)","ヽ (o´∀`) ﾉ ♪ ♬","(ﾉ ≧ ∀ ≦) ﾉ","♪ ヽ (^^ ヽ) ♪","♪ (/ _ _) / ♪","♪ ♬ ((d⌒ω⌒b)) ♬ ♪","└ (￣-￣└))","((┘￣ω￣) ┘","√ (￣ ‥ ￣√)","└ (＾＾) ┐","┌ (＾＾) ┘","＼ (￣ ▽ ￣) ＼","／ (￣ ▽ ￣) ／","(￣ ▽ ￣) /♫•*¨*•.¸¸♪","(^ _ ^ ♪)","(~ ˘ ▽ ˘) ~","~ (˘ ▽ ˘ ~)","ヾ (⌐ ■ _ ■) ノ ♪","(〜￣ △ ￣) 〜","(~ ‾ ▽ ‾) ~","~ (˘ ▽ ˘) ~","乁 (• ω • 乁)","(｢• ω •)｢","⁽⁽◝ (• ω •) ◜⁾⁾","✺◟ (• ω •) ◞✺","♬ ♫ ♪ ◖ (● o ●) ◗ ♪ ♫ ♬","(˘ ɜ˘) ♬ ♪ ♫","♪♪♪ ヽ (ˇ∀ˇ) ゞ"}},
            {"Games", new string[]{"(^^) p _____ | _o ____ q (^^)","(／ O ^) / ° ⊥ ＼ (^ o＼)","! (; ﾟ o ﾟ) o / ￣￣￣￣￣￣￣ ~> ﾟ)))) 彡","ヽ (^ o ^) ρ┳┻┳ ° σ (^ o ^) ノ","(／ _ ^) ／ ● ＼ (^ _ ＼)","“((≡ | ≡)) _ ／ ＼ _ ((≡ | ≡))”","(ノ -_-) ノ ﾞ _ □ VS □ _ ヾ (^ – ^ ヽ)","ヽ (； ^ ^) ノ ﾞ ．．．…___ 〇","(= O * _ *) = OQ (* _ * Q)","Ю ○ 三 ＼ (￣ ^ ￣＼)"}},
            {"Faces", new string[]{"(͡ ° ͜ʖ ͡ °)","(͡ ° ʖ̯ ͡ °)","(͠ ° ͟ʖ ͡ °)","(͡ᵔ ͜ʖ ͡ᵔ)","(. • ́ _ʖ • ̀.)","(ఠ ͟ʖ ఠ)","(͡ಠ ʖ̯ ͡ಠ)","(ಠ ʖ̯ ಠ)","(ಠ ͜ʖ ಠ)","(ಥ ʖ̯ ಥ)","(͡ • ͜ʖ ͡ •)","(･ ิ ͜ʖ ･ ิ)","(͡ ͜ʖ ͡)","(≖ ͜ʖ≖)","(ʘ ʖ̯ ʘ)","(ʘ ͟ʖ ʘ)","(ʘ ͜ʖ ʘ)","(; ´ ༎ ຶ ٹ ༎ ຶ `)"}}
        };

        private void EmojiManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) this.DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText(Settings["emoji_data_path"].ToString(), JsonConvert.SerializeObject(EmojiData, Formatting.Indented));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void EmojiManagerForm_Load(object sender, EventArgs e)
        {
            // Creating/reading the settings data
            if (!File.Exists(".\\settings.json"))
            {
                Settings.Add("emoji_data_path", ".\\emoji_data.json");
                Settings.Add("show_window_when_start", true);
                File.WriteAllText(".\\settings.json", JsonConvert.SerializeObject(Settings, Formatting.Indented));
            }
            else
            {
                string SettingsText = File.ReadAllText(".\\settings.json");
                try
                {
                    Settings = JsonConvert.DeserializeObject<Dictionary<string, object>>(SettingsText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while reading the settings data file! Please check the Settings data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Creating/reading emoji data
            if (!File.Exists(Settings["emoji_data_path"].ToString())) File.WriteAllText(Settings["emoji_data_path"].ToString(), JsonConvert.SerializeObject(EmojiData, Formatting.Indented));
            else
            {
                string EmojiText = File.ReadAllText(Settings["emoji_data_path"].ToString());
                try
                {
                    EmojiData = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(EmojiText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while reading the emoji data file! Please check the emoji data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            CategoryLoad();
        }

        bool loading = false;
        int index = 0;

        void CategoryLoad()
        {
            loading = true;
            listBox1.Items.Clear();
            foreach (var Emojis in EmojiData)
            {
                listBox1.Items.Add(Emojis.Key);
            }
            listBox1.SelectedItem = listBox1.Items[index];
            loading = false;
            EmojiLoad();
        }

        void EmojiLoad()
        {
            if (!loading)
            {
                panel1.Controls.Clear();
                var Emojis = EmojiData[listBox1.SelectedItem.ToString()];
                int x = 0, y = 0;
                for (var i = 0; i < Emojis.Length; i++)
                {
                    if (x == 3)
                    {
                        x = 0;
                        y++;
                    }
                    var Emoji = Emojis[i];
                    var panel = new Panel();
                    panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    panel.Location = new Point(8 + x * 175, 8 + y * 46);
                    panel.Size = new Size(170, 41);
                    var EmojiLabel = new Label();
                    EmojiLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
                    EmojiLabel.Location = new System.Drawing.Point(0, 0);
                    EmojiLabel.Size = new System.Drawing.Size(99, 39);
                    EmojiLabel.TabIndex = 0;
                    EmojiLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    EmojiLabel.Text = Emoji;
                    var RemoveButton = new Button();
                    RemoveButton.Location = new System.Drawing.Point(105, 0);
                    RemoveButton.Name = "RemoveButton" + i.ToString();
                    RemoveButton.Size = new System.Drawing.Size(63, 39);
                    RemoveButton.TabIndex = 8;
                    RemoveButton.Text = "Remove";
                    RemoveButton.Click += new EventHandler(Remove);
                    RemoveButton.UseVisualStyleBackColor = true;
                    panel.Controls.Add(EmojiLabel);
                    panel.Controls.Add(RemoveButton);
                    panel1.Controls.Add(panel);
                    x++;
                }
            }
        }

        void Remove(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove this emoji?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var Emojis = (string[])EmojiData[listBox1.SelectedItem.ToString()];
                int i = Convert.ToInt32(((Button)sender).Name.Substring(12));
                Emojis = Emojis.Where((source, id) => id != i).ToArray();
                EmojiData[listBox1.SelectedItem.ToString()] = Emojis;
                EmojiLoad();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Remove this category?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                for (var i = 0; i < listBox1.Items.Count; i++)
                {
                    if (listBox1.Items[i].ToString() == listBox1.SelectedItem.ToString())
                    {
                        index = i;
                        break;
                    }
                }
                EmojiData.Remove(listBox1.SelectedItem.ToString());
                CategoryLoad();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            EmojiLoad();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var form = new PromptForm("Type the category name:", "Add Category"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    EmojiData.Add(form.Value, new string[] { });
                    index = EmojiData.Count - 1;
                    CategoryLoad();
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (var form = new PromptForm("Type the emoji name:", "Add Emoji"))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var Emojis = (string[])EmojiData[listBox1.SelectedItem.ToString()];
                    Array.Resize(ref Emojis, Emojis.Length + 1);
                    Emojis[Emojis.GetUpperBound(0)] = form.Value;
                    EmojiData[listBox1.SelectedItem.ToString()] = Emojis;
                    EmojiLoad();
                }
            }
        }
    }
}
