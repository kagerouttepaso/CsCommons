using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

//下記のコードのprojectnameをリネームする
//namespace projectname.Properties{
//	[System.Configuration.SettingsProvider(typeof(Commons.Settings.ConfigFileSettingsProvider))]
//	internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
//	}
//}

namespace Commons.Settings{

	/// <summary>
	/// Protraのconfディレクトリに設定を保存するための設定プロバイダー
	/// </summary>
	public class ConfigFileSettingsProvider : SettingsProvider {
        private readonly string configFile = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName) + ".config");

		/// <summary>
		/// プロバイダーを初期化する。
		/// </summary>
		/// <param name="name"></param>
		/// <param name="config"></param>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config) {
			base.Initialize("ConfigFileSettingsProvider", config);
		}

		/// <summary>
		/// 現在実行中のアプリケーションの名前を取得または設定する。
		/// </summary>
		public override string ApplicationName {
            get { return AppDomain.CurrentDomain.FriendlyName; }
            set { }
		}

		/// <summary>
		/// シリアライズ可能なKeyValuePair
		/// </summary>
		/// <typeparam name="K">キーの型</typeparam>
		/// <typeparam name="V">値の型</typeparam>
		[Serializable]
		public struct KeyValuePair<K, V> {
			/// <summary>
			/// コンストラクタ
			/// </summary>
			/// <param name="key">キー</param>
			/// <param name="value">値</param>
			public KeyValuePair(K key, V value)
				: this() {
				Key = key;
				Value = value;
			}
			/// <summary>
			/// キーを取得または設定する。
			/// </summary>
			public K Key { get; set; }
			/// <summary>
			/// 値を取得または設定する。
			/// </summary>
			public V Value { get; set; }
		}

		/// <summary>
		/// 設定ファイルを読んで指定された設定プロパティグループの値のコレクションを返す。
		/// </summary>
		/// <param name="context">現在のアプリケーションの使い方を記述しているSettingsContext。</param>
		/// <param name="collection">値の取得対象となる設定プロパティグループを格納しているSettingsPropertyCollection。</param>
		/// <returns>設定プロパティグループの値を格納しているSettingsPropertyValueCollection。</returns>
		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection) {
			var dictionary = new Dictionary<string, Object>();
			var valueCollection = new SettingsPropertyValueCollection();
			var selializer = new XmlSerializer(typeof(List<KeyValuePair<string, object>>));
			//デシリアライズ
			try {
				using(var reader = File.OpenText(configFile))
					((List<KeyValuePair<string, Object>>)selializer.Deserialize(reader))
						.ForEach(pair => {
							dictionary[pair.Key] = pair.Value;
						});
			} catch(Exception e) {
				if(!(e is IOException || e is InvalidOperationException))
					throw;
			}
			//コレクションの代入う
			foreach(SettingsProperty property in collection)
				valueCollection.Add(new SettingsPropertyValue(property) {
					SerializedValue = dictionary.ContainsKey(property.Name)
										  ? dictionary[property.Name]
										  : property.DefaultValue,
					IsDirty = false
				});
			return valueCollection;
		}

		/// <summary>
		/// 指定された設定プロパティグループの値を設定ファイルに保存します。
		/// </summary>
		/// <param name="context">現在のアプリケーションの使い方を記述しているSettingsContext。</param>
		/// <param name="collection">保存する設定プロパティグループを格納しているSettingsPropertyCollection。</param>
		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection) {
			var kvList = new List<KeyValuePair<string, Object>>();
			var selializer = new XmlSerializer(kvList.GetType());
			foreach(SettingsPropertyValue value in collection)
				kvList.Add(new KeyValuePair<string, object>(value.Name, value.SerializedValue));
			using(var stream = File.CreateText(configFile))
				selializer.Serialize(stream, kvList);
		}
	}
}
