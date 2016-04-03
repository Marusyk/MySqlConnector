﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using static System.FormattableString;

namespace MySql.Data.MySqlClient
{
	public sealed class MySqlConnectionStringBuilder : DbConnectionStringBuilder
	{
		public MySqlConnectionStringBuilder()
		{
		}

		public MySqlConnectionStringBuilder(string connectionString)
		{
			ConnectionString = connectionString;
		}

		public string Server
		{
			get { return MySqlConnectionStringOption.Server.GetValue(this); }
			set { MySqlConnectionStringOption.Server.SetValue(this, value); }
		}

		public string UserID
		{
			get { return MySqlConnectionStringOption.UserID.GetValue(this); }
			set { MySqlConnectionStringOption.UserID.SetValue(this, value); }
		}

		public string Password
		{
			get { return MySqlConnectionStringOption.Password.GetValue(this); }
			set { MySqlConnectionStringOption.Password.SetValue(this, value); }
		}

		public string Database
		{
			get { return MySqlConnectionStringOption.Database.GetValue(this); }
			set { MySqlConnectionStringOption.Database.SetValue(this, value); }
		}

		public uint Port
		{
			get { return MySqlConnectionStringOption.Port.GetValue(this); }
			set { MySqlConnectionStringOption.Port.SetValue(this, value); }
		}

		public bool AllowUserVariables
		{
			get { return MySqlConnectionStringOption.AllowUserVariables.GetValue(this); }
			set { MySqlConnectionStringOption.AllowUserVariables.SetValue(this, value); }
		}

		public string CharacterSet
		{
			get { return MySqlConnectionStringOption.CharacterSet.GetValue(this); }
			set { MySqlConnectionStringOption.CharacterSet.SetValue(this, value); }
		}

		public bool UseCompression
		{
			get { return MySqlConnectionStringOption.UseCompression.GetValue(this); }
			set { MySqlConnectionStringOption.UseCompression.SetValue(this, value); }
		}

		public bool Pooling
		{
			get { return MySqlConnectionStringOption.Pooling.GetValue(this); }
			set { MySqlConnectionStringOption.Pooling.SetValue(this, value); }
		}

		public bool ConnectionReset
		{
			get { return MySqlConnectionStringOption.ConnectionReset.GetValue(this); }
			set { MySqlConnectionStringOption.ConnectionReset.SetValue(this, value); }
		}

		public uint MinimumPoolSize
		{
			get { return MySqlConnectionStringOption.MinimumPoolSize.GetValue(this); }
			set { MySqlConnectionStringOption.MinimumPoolSize.SetValue(this, value); }
		}

		public uint MaximumPoolSize
		{
			get { return MySqlConnectionStringOption.MaximumPoolSize.GetValue(this); }
			set { MySqlConnectionStringOption.MaximumPoolSize.SetValue(this, value); }
		}

		public override bool ContainsKey(string key)
		{
			var option = MySqlConnectionStringOption.TryGetOptionForKey(key);
			return option != null && base.ContainsKey(option.Key);
		}

		public override bool Remove(string key)
		{
			var option = MySqlConnectionStringOption.TryGetOptionForKey(key);
			return option != null && base.Remove(option.Key);
		}

		public override object this[string key]
		{
			get { return MySqlConnectionStringOption.GetOptionForKey(key).GetObject(this); }
			set { base[MySqlConnectionStringOption.GetOptionForKey(key).Key] = Convert.ToString(value, CultureInfo.InvariantCulture); }
		}
	}

	internal abstract class MySqlConnectionStringOption
	{
		public static readonly MySqlConnectionStringOption<string> Server;
		public static readonly MySqlConnectionStringOption<string> UserID;
		public static readonly MySqlConnectionStringOption<string> Password;
		public static readonly MySqlConnectionStringOption<string> Database;
		public static readonly MySqlConnectionStringOption<uint> Port;
		public static readonly MySqlConnectionStringOption<bool> AllowUserVariables;
		public static readonly MySqlConnectionStringOption<string> CharacterSet;
		public static readonly MySqlConnectionStringOption<bool> UseCompression;
		public static readonly MySqlConnectionStringOption<bool> Pooling;
		public static readonly MySqlConnectionStringOption<bool> ConnectionReset;
		public static readonly MySqlConnectionStringOption<uint> MinimumPoolSize;
		public static readonly MySqlConnectionStringOption<uint> MaximumPoolSize;

		public static MySqlConnectionStringOption TryGetOptionForKey(string key)
		{
			MySqlConnectionStringOption option;
			return s_options.TryGetValue(key, out option) ? option : null;
		}

		public static MySqlConnectionStringOption GetOptionForKey(string key)
		{
			var option = TryGetOptionForKey(key);
			if (option == null)
				throw new InvalidOperationException(Invariant($"Option '{key}' not supported."));
			return option;
		}

		public string Key => m_keys[0];

		public abstract object GetObject(MySqlConnectionStringBuilder builder);

		protected MySqlConnectionStringOption(IReadOnlyList<string> keys)
		{
			m_keys = keys;
		}

		private static void AddOption(MySqlConnectionStringOption option)
		{
			foreach (string key in option.m_keys)
				s_options.Add(key, option);
		}

		static MySqlConnectionStringOption()
		{
			s_options = new Dictionary<string, MySqlConnectionStringOption>(StringComparer.OrdinalIgnoreCase);

			AddOption(Server = new MySqlConnectionStringOption<string>(
				keys: new[] { "Server", "Host", "Data Source", "DataSource", "Address", "Addr", "Network Address" },
				defaultValue: ""));

			AddOption(UserID = new MySqlConnectionStringOption<string>(
				keys: new[] { "User Id", "UserID", "Username", "Uid", "User name", "User" },
				defaultValue: ""));

			AddOption(Password = new MySqlConnectionStringOption<string>(
				keys: new[] { "Password", "pwd" },
				defaultValue: ""));

			AddOption(Database = new MySqlConnectionStringOption<string>(
				keys: new[] { "Database", "Initial Catalog" },
				defaultValue: ""));

			AddOption(Port = new MySqlConnectionStringOption<uint>(
				keys: new[] { "Port" },
				defaultValue: 3306u));

			AddOption(AllowUserVariables = new MySqlConnectionStringOption<bool>(
				keys: new[] { "AllowUserVariables", "Allow User Variables" },
				defaultValue: false));

			AddOption(CharacterSet = new MySqlConnectionStringOption<string>(
				keys: new[] { "CharSet", "Character Set", "CharacterSet" },
				defaultValue: ""));

			AddOption(UseCompression = new MySqlConnectionStringOption<bool>(
				keys: new[] { "Compress", "Use Compression", "UseCompression" },
				defaultValue: false,
				coerce: value =>
				{
					if (value)
						throw new InvalidOperationException("Compression not supported.");
					return value;
				}));

			AddOption(Pooling = new MySqlConnectionStringOption<bool>(
				keys: new[] { "Pooling" },
				defaultValue: true));

			AddOption(ConnectionReset = new MySqlConnectionStringOption<bool>(
				keys: new[] { "Connection Reset", "ConnectionReset" },
				defaultValue: false));

			AddOption(MinimumPoolSize = new MySqlConnectionStringOption<uint>(
				keys: new[] { "Minimum Pool Size", "Min Pool Size", "MinimumPoolSize", "minpoolsize" },
				defaultValue: 0,
				coerce: value =>
				{
					if (value != 0)
						throw new InvalidOperationException("Non-zero MinimumPoolSize not supported.");
					return value;
				}));

			AddOption(MaximumPoolSize = new MySqlConnectionStringOption<uint>(
				keys: new[] { "Maximum Pool Size", "Max Pool Size", "MaximumPoolSize", "maxpoolsize" },
				defaultValue: 100));
		}

		static readonly Dictionary<string, MySqlConnectionStringOption> s_options;

		readonly IReadOnlyList<string> m_keys;
	}

	internal sealed class MySqlConnectionStringOption<T> : MySqlConnectionStringOption
	{
		public MySqlConnectionStringOption(IReadOnlyList<string> keys, T defaultValue, Func<T, T> coerce = null)
			: base(keys)
		{
			m_defaultValue = defaultValue;
			m_coerce = coerce;
		}

		public T GetValue(MySqlConnectionStringBuilder builder)
		{
			object objectValue;
			return builder.TryGetValue(Key, out objectValue) ? (T) Convert.ChangeType(objectValue, typeof(T), CultureInfo.InvariantCulture) : m_defaultValue;
		}

		public void SetValue(MySqlConnectionStringBuilder builder, T value)
		{
			builder[Key] = m_coerce != null ? m_coerce(value) : value;
		}

		public override object GetObject(MySqlConnectionStringBuilder builder)
		{
			return GetValue(builder);
		}

		readonly T m_defaultValue;
		readonly Func<T, T> m_coerce;
	}
}
