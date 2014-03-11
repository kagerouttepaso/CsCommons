using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Commons.DesignPattern {
	/// <summary>
	/// 拡張メソッド集
	/// </summary>
	static class ObjectExtenslons {
		/// <summary>
		/// オブジェクトのプロパティ値を名前から取得
		/// </summary>
		/// <param name="item">オブジェクト</param>
		/// <param name="propertyName">プロパティ名</param>
		/// <returns>オブジェクトのプロパティの値</returns>
		public static object Eval(this object item, string propertyName) {
			var propertyInfo = item.GetType().GetProperty(propertyName);
			return propertyInfo == null ? null : propertyInfo.GetValue(item, null);
		}

		/// <summary>
		/// Expressionからメンバー名を取得
		/// </summary>
		/// <typeparam name="ObjectType">このメソッドを呼び出したオブジェクトの型</typeparam>
		/// <typeparam name="MemberType">オブジェクトのメンバーの型</typeparam>
		/// <param name="this">呼び出し元のオブジェクト</param>
		/// <param name="expression">Expression</param>
		/// <returns>オブジェクトのメンバの名前</returns>
		public static string GetMemberName<ObjectType, MemberType>(this ObjectType @this, Expression<Func<ObjectType, MemberType>> expression) {
			return ((MemberExpression)expression.Body).Member.Name;
		}
	}

	/// <summary>
	/// 更新を監視される側
	/// </summary>
	public abstract class Observable {
		/// <summary>
		/// バインドされたObserverの更新関数を呼び出す
		/// </summary>
		public event Action<string> Updata;

		/// <summary>
		/// イベントの呼び出し関数
		/// </summary>
		/// <param name="propertyName">このメソッドを呼び出したプロパティの値</param>
		protected void RaiseUpdate([CallerMemberName] string propertyName = "") {
			if(Updata != null) {
				Updata(propertyName);
			}
		}
	}

	/// <summary>
	/// 更新を監視する側
	/// </summary>
	public abstract class Observer<ObservableType> where ObservableType : Observable {
		/// <summary>
		/// 各種アップデートコマンドを保管する
		/// </summary>
		Dictionary<string, Action<object>> updateExpressions = new Dictionary<string, Action<object>>();

		/// <summary>
		/// バインドしたObservable
		/// </summary>
		ObservableType dataSource = null;

		/// <summary>
		/// Observableをバインドする
		/// </summary>
		public ObservableType DataSource {
			set {
				if(dataSource != null)
					dataSource.Updata -= Update;
				dataSource = value;
				value.Updata += Update;
			}
			get { return dataSource; }
		}

		/// <summary>
		/// アップデートメソッドをExpressionをもとに登録する
		/// </summary>
		/// <typeparam name="PropertyType">Observableのプロパティ</typeparam>
		/// <param name="propertyExpression">対応するプロパティ</param>
		/// <param name="updateAction">更新時に呼び出すメソッド</param>
		protected void AddUpdateAction<PropertyType>(Expression<Func<ObservableType, PropertyType>> propertyExpression, Action<object> updateAction) {
			_AddUpdateAction(dataSource.GetMemberName(propertyExpression), updateAction);
		}

		/// <summary>
		/// アップデートメソッドをExpressionをもとに登録する(横着版)
		/// </summary>
		/// <typeparam name="PropertyType">Observableのプロパティ</typeparam>
		/// <param name="propertyExpression">対応するプロパティ</param>
		/// <param name="updateAction">更新時に呼び出すメソッド</param>
		public void AddUpdateAction<PropertyType>(Expression<Func<ObservableType, PropertyType>> propertyExpression, Action<PropertyType> updateAction) {
			Action<object> rapAction = obj => {
				var prop = (PropertyType)obj;
				updateAction(prop);
			};
			_AddUpdateAction(dataSource.GetMemberName(propertyExpression), rapAction);
		}

		/// <summary>
		/// アップデートメソッドを文字列をもとに登録する
		/// </summary>
		/// <param name="propertyName">対応するプロパティ</param>
		/// <param name="updateAction">更新時に呼び出すメソッド</param>
		void _AddUpdateAction(string propertyName, Action<object> updateAction) {
			updateExpressions[propertyName] = updateAction;
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="propertyName">更新されたプロパティ</param>
		void Update(string propertyName) {
			Action<object> updateAction;
			if(updateExpressions.TryGetValue(propertyName, out updateAction))
				updateAction(dataSource.Eval(propertyName));
		}
	}
}
