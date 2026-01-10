const TitloviUniqueId = "6e25df50-638e-4109-a50b-03c14fc93fdd"

export default function(view, _) {

  view.addEventListener('viewshow', function() {
    Dashboard.showLoadingMsg();

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(function(config) {
      Dashboard.hideLoadingMsg();
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

}
