async function sendRequest(endpoint, body = undefined) {
  const response = await fetch(ApiClient.getUrl(endpoint), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-Emby-Token': ApiClient.accessToken()
    },
    body: body
  });

  if (!response.ok) {
    throw new Error();
  }
  return response;
}

export default function(view, _) {
  const TitloviUniqueId = "6e25df50-638e-4109-a50b-03c14fc93fdd"

  view.addEventListener('viewshow', function() {
    Dashboard.showLoadingMsg();
    const page = this;

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(function(config) {
      page.querySelector('#username').value = config.Username || '';
      page.querySelector('#password').value = config.Password || '';

      if (config.Token) {
        page.querySelector('#token').value = config.Token.Token;
      }

      Dashboard.hideLoadingMsg();
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

  view.querySelector('#TitloviConfigForm').addEventListener('submit', function(e) {
    e.preventDefault();
    Dashboard.showLoadingMsg();

    const form = this;

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(async function(config) {
      const username = form.querySelector('#username').value.trim()
      const password = form.querySelector('#password').value.trim()

      try {
        let response = await sendRequest('Titlovi/GetToken', JSON.stringify({
          username,
          password
        }))

        config.Username = username
        config.Password = password
        config.Token = await response.json()

        view.querySelector('#username').value = config.Username
        view.querySelector('#password').value = config.Password
        view.querySelector('#token').value = config.Token

        ApiClient.updatePluginConfiguration(TitloviUniqueId, config).then(function(result) {
          Dashboard.processPluginConfigurationUpdateResult(result);
          Dashboard.hideLoadingMsg();
        }).catch(function() {
          Dashboard.processErrorResponse({ statusText: "Failed to update plugin configuration" });
          Dashboard.hideLoadingMsg();
        });
      } catch (error) {
        Dashboard.alert({ message: "Authentication has failed, please check your credentials and try again!", title: "Error" });
        Dashboard.hideLoadingMsg();
      }
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

  view.querySelector('#TitloviDeleteToken').addEventListener('click', function(e) {
    e.preventDefault();
    Dashboard.showLoadingMsg();

    ApiClient.getPluginConfiguration(TitloviUniqueId).then(async function() {
      try {
        await sendRequest('Titlovi/InvalidateToken')
        view.querySelector('#token').value = ''
        Dashboard.hideLoadingMsg();
      } catch (error) {
        Dashboard.alert({ message: error, title: "Error" });
        Dashboard.hideLoadingMsg();
      }
    }).catch(function() {
      Dashboard.hideLoadingMsg();
      Dashboard.processErrorResponse({ statusText: "Failed to load plugin configuration" });
    });
  });

}