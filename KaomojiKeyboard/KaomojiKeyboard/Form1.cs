using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace KaomojiKeyboard
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
 
        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        Dictionary<string, object> Settings = new Dictionary<string, object>();
        Dictionary<string, Dictionary<string, string>> Hotkeys = new Dictionary<string, Dictionary<string, string>>();
        bool closing = false;
        string addedKey = "";

        public Form1()
        {
            InitializeComponent();
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("KaomojiKeyboard") != null) rkApp.SetValue("KaomojiKeyboard", Application.ExecutablePath);
            int id = 0;
            // Creating/reading the hotkeys data
            if (!File.Exists(".\\hotkeys.json"))
            {
                Hotkeys.Add("show_keyboard", new Dictionary<string, string>(){
                    {"key", "1.OemPeriod"}
                });
                File.WriteAllText(".\\hotkeys.json", JsonConvert.SerializeObject(Hotkeys, Formatting.Indented));
            }
            else
            {
                string HotkeysText = File.ReadAllText(".\\hotkeys.json");
                try
                {
                    Hotkeys = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(HotkeysText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error has occurred while reading the hotkeys data file! Please check the Settings data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            var ShowKey = Hotkeys["show_keyboard"];
            string[] ShowKeyData = ShowKey["key"].Split('.');
            Keys showkey;
            Enum.TryParse(ShowKeyData[1], out showkey);
            RegisterHotKey(this.Handle, id, Convert.ToInt32(ShowKeyData[0]), showkey.GetHashCode());
            addedKey = ShowKey["key"];
            Hotkeys.Add(ShowKey["key"], new Dictionary<string, string>() { { "type", "show_keyboard_window" } });
            foreach (var HotkeyData in Hotkeys)
            {
                if (HotkeyData.Key.Contains("."))
                {
                    string[] KeyData = HotkeyData.Key.Split('.');
                    Keys key;
                    Enum.TryParse(KeyData[1], out key);
                    RegisterHotKey(this.Handle, id, Convert.ToInt32(KeyData[0]), key.GetHashCode());
                }
            }
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                try
                {
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);
                    int id = m.WParam.ToInt32();
                    var KeyVocab = new Dictionary<string, int>() { { "Alt", 1 }, { "Control", 2 }, { "Shift", 4 }, { "WinKey", 8 } };
                    Dictionary<string, string> KeyData = Hotkeys[KeyVocab[modifier.ToString()].ToString() + "." + key.ToString()];
                    if (KeyData["type"] == "show_keyboard_window") this.Show();
                    else if (KeyData["type"] == "emoji") SendKeys.Send(Regex.Replace(KeyData["emoji"], "[+^%~()]", "{$0}"));
                }
                catch
                {

                }
            }
        }

        const int WS_EX_NOACTIVATE = 0x08000000;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                param.ExStyle |= WS_EX_NOACTIVATE;
                return param;
            }
        }

        Dictionary<string, string[]> DefaultEmojiData = new Dictionary<string, string[]>()
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

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("KaomojiKeyboard").Length > 1)
            {
                MessageBox.Show("There's another KaomojiKeyboard running on this computer. Close that one first and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                closing = true;
                Application.Exit();
            }
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
            // Reading emojis
            foreach (var Emojis in EmojiData)
            {
                tabControl1.TabPages.Add(Emojis.Key);
            }
            load();
        }

        private void EmojiClick(object sender, EventArgs e)
        {
            SendKeys.Send(Regex.Replace((sender as Button).Text, "[+^%~()]", "{$0}"));
        }

        bool loading = false;

        void load()
        {
            if (!loading)
            {
                tabControl1.SelectedTab.AutoScroll = true;
                if (EmojiData.ContainsKey(tabControl1.SelectedTab.Text))
                {
                    var Emojis = EmojiData[tabControl1.SelectedTab.Text];
                    int x = 0, y = 0;
                    foreach (string Emoji in Emojis)
                    {
                        if (x == 3)
                        {
                            x = 0;
                            y++;
                        }
                        Button button = new Button();
                        button.Location = new Point(8 + x * 235, 6 + y * 30);
                        button.Size = new Size(230, 25);
                        button.Text = Emoji;
                        button.Click += new EventHandler(EmojiClick);
                        tabControl1.SelectedTab.Controls.Add(button);
                        x++;
                    }
                }
                else
                {
                    MessageBox.Show("This specified category can't be found in the emoji data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            load();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon1.BalloonTipTitle = "KaomojiKeyboard";
                var KeyVocab = new Dictionary<string, string>() { { "1", "Alt" }, { "2", "Control" }, { "4", "Shift" }, { "8", "Windows" } };
                var ShowKey = Hotkeys["show_keyboard"];
                string[] ShowKeyData = ShowKey["key"].Split('.');
                notifyIcon1.BalloonTipText = "The keyboard is currently running on the background. You can press " + KeyVocab[ShowKeyData[0]] + " + " + ShowKeyData[1]
                    .Replace("OemPeriod", ".")
                    .Replace("Oemcomma", ",")
                    .Replace("OemBackslash", "\\")
                    .Replace("OemCloseBrackets", "]")
                    .Replace("OemMinus", "-")
                    .Replace("OemOpenBrackets", "[")
                    .Replace("OemSemicolon", ";")
                    .Replace("OemQuotes", "'")
                    .Replace("Oem", "")
                + " to reopen the keyboard.";
                notifyIcon1.ShowBalloonTip(5000);
            }
        }

        private void showKeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }

        private void refreshEmojiDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Creating/reading emoji data
            loading = true;
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
            // Reading emojis
            tabControl1.TabPages.Clear();
            foreach (var Emojis in EmojiData)
            {
                tabControl1.TabPages.Add(Emojis.Key);
            }
            loading = false;
            load();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new SettingsForm())
            {
                this.TopMost = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
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
                    loading = true;
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
                    // Reading emojis
                    tabControl1.TabPages.Clear();
                    foreach (var Emojis in EmojiData)
                    {
                        tabControl1.TabPages.Add(Emojis.Key);
                    }
                    loading = false;
                    load();
                    int id = 0;
                    UnregisterHotKey(this.Handle, id);
                    // Creating/reading the hotkeys data
                    if (!File.Exists(".\\hotkeys.json"))
                    {
                        Hotkeys.Add("show_keyboard", new Dictionary<string, string>(){
                            {"key", "1.OemPeriod"}
                        });
                        File.WriteAllText(".\\hotkeys.json", JsonConvert.SerializeObject(Hotkeys, Formatting.Indented));
                    }
                    else
                    {
                        string HotkeysText = File.ReadAllText(".\\hotkeys.json");
                        try
                        {
                            Hotkeys = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(HotkeysText);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error has occurred while reading the hotkeys data file! Please check the Settings data file and try again!\n\nError information:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    var ShowKey = Hotkeys["show_keyboard"];
                    string[] ShowKeyData = ShowKey["key"].Split('.');
                    Keys showkey;
                    Enum.TryParse(ShowKeyData[1], out showkey);
                    RegisterHotKey(this.Handle, id, Convert.ToInt32(ShowKeyData[0]), showkey.GetHashCode());
                    Hotkeys.Remove(addedKey);
                    addedKey = ShowKey["key"];
                    Hotkeys.Add(ShowKey["key"], new Dictionary<string, string>() { { "type", "show_keyboard_window" } });
                    foreach (var HotkeyData in Hotkeys)
                    {
                        if (HotkeyData.Key.Contains("."))
                        {
                            string[] KeyData = HotkeyData.Key.Split('.');
                            Keys key;
                            Enum.TryParse(KeyData[1], out key);
                            RegisterHotKey(this.Handle, id, Convert.ToInt32(KeyData[0]), key.GetHashCode());
                        }
                    }
                }
                this.TopMost = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (Settings.ContainsKey("show_window_when_start") && (bool)Settings["show_window_when_start"] == false) this.Hide();
        }

        private void resetEmojiDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure that you want to reset the emoji database to DEFAULT? This action will be IRREVERSIBLE!", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                EmojiData = DefaultEmojiData;
                File.WriteAllText(Settings["emoji_data_path"].ToString(), JsonConvert.SerializeObject(EmojiData, Formatting.Indented));
                // Reading emojis
                loading = true;
                tabControl1.TabPages.Clear();
                foreach (var Emojis in EmojiData)
                {
                    tabControl1.TabPages.Add(Emojis.Key);
                }
                loading = false;
                load();
                MessageBox.Show("Successfully reset the emoji data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("KaomojiKeyboard v1.0.0\nBuild date: February 16, 2022\nMade by Meir/Nico Levianth/LilShieru/S1est4.\n\nDefault database is crawled from:\nhttps://kynguyencongnghe.com/tong-hop-kaomoji-japanese-emoticons\nAdditional libraries used: Newtonsoft.JSON\n\n" + (JsonConvert.SerializeObject(EmojiData, Formatting.Indented) == JsonConvert.SerializeObject(DefaultEmojiData, Formatting.Indented) ? "The current database is as same as the default database (you haven't modified anything yet)." : "The current database has been modified from the default database.") + "\n\nI also copied some scripts from StackOverflow, shout out to ones who made it (I don't remember them) :')\nAlso, shout out to Facebook/100076324843858 for the idea!\n\nMade with Microsoft Visual C# 2010 Express on Windows 10 20H2.\n\n[GitHub]\nSource code: https://www.github.com/LilShieru/KaomojiKeyboard\nUser: https://www.github.com/LilShieru", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void emojiManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new EmojiManagerForm())
            {
                this.TopMost = false;
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    // Creating/reading emoji data
                    loading = true;
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
                    // Reading emojis
                    tabControl1.TabPages.Clear();
                    foreach (var Emojis in EmojiData)
                    {
                        tabControl1.TabPages.Add(Emojis.Key);
                    }
                    loading = false;
                    load();
                }
                this.TopMost = true;
            }
        }

        private void openEmojiDataFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", Settings["emoji_data_path"].ToString());
        }

        private void showEmojiDataFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "/select,\"" + Settings["emoji_data_path"].ToString() + "\"");
        }
    }
}
