using System;
using NPoco;

namespace Our.Umbraco.SecretStore.Persistence.Dtos
{
    [TableName("umbracoKeyValue")]
    [PrimaryKey("key", AutoIncrement = false)]
    internal class SecretDto
    {
        [Column("key")]
        public string Key { get; set; }

        [Column("value")]
        public string Secret { get; set; }

        [Column("updated")]
        public DateTime? UpdateDate { get; set; }
    }
}