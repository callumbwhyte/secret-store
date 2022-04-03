using System.Net.Http.Formatting;
using System.Web.Http.ModelBinding;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Web.WebApi.Filters;
using Applications = Umbraco.Core.Constants.Applications;
using TreeGroups = Umbraco.Core.Constants.Trees.Groups;

namespace Our.Umbraco.SecretStore.Web.Trees
{
    [PluginController(Constants.SectionAlias)]
    [Tree(Applications.Settings, Constants.SectionAlias, TreeGroup = TreeGroups.Settings, SortOrder = 15)]
    public class SecretStoreTreeController : TreeController
    {
        protected override TreeNode CreateRootNode(FormDataCollection queryStrings)
        {
            var node = base.CreateRootNode(queryStrings);

            node.RoutePath = $"{SectionAlias}/{TreeAlias}/overview";

            node.Icon = "icon-lock";

            node.HasChildren = false;

            return node;
        }

        protected override MenuItemCollection GetMenuForNode(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }

        protected override TreeNodeCollection GetTreeNodes(string id, [ModelBinder(typeof(HttpQueryStringModelBinder))] FormDataCollection queryStrings)
        {
            return new TreeNodeCollection();
        }
    }
}