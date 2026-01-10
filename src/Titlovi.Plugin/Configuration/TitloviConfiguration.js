const TitloviUniqueId = "6e25df50-638e-4109-a50b-03c14fc93fdd"

export default function(view, _) {

  view.addEventListener('viewshow', function() {
    Dashboard.showLoadingMsg();

    const page = this;

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(function(config) {
      page.querySelector('#username').value = config.Username || '';
      page.querySelector('#password').value = config.Password || '';

      Dashboard.hideLoadingMsg();
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

  view.querySelector('#TitloviConfigForm').addEventListener('submit', function(e) {
    e.preventDefault();

    const form = this;

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(function(config) {
      const username = form.querySelector('#username').value.trim();
      const password = form.querySelector('#password').value.trim();

      fetch(ApiClient.getUrl('Titlovi/GetToken'), {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'X-Emby-Token': ApiClient.accessToken()
        },
        body: JSON.stringify({
          username,
          password
        })
      }).then(response => {
        console.log(response)
        if (response.ok) {
          return response.text()
        } else {
          Dashboard.alert({ message: "Authentication has failed, please check your credentials and try again!", title: "Error" });
          Dashboard.hideLoadingMsg();
          throw new Error('Authentication failed');
        }
      }).then(data => {
        config.Username = username
        config.Password = password

        console.log(data);

        ApiClient.updatePluginConfiguration(TitloviUniqueId, config).then(function(result) {
          Dashboard.processPluginConfigurationUpdateResult(result);
        }).catch(function() {
          Dashboard.processErrorResponse({ statusText: "Failed to update plugin configuration" });
        });
      }).catch(error => {
        Dashboard.alert({ message: error, title: "Error" })
        Dashboard.hideLoadingMsg();
      });
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

}
