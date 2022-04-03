# Umbraco Secret Store

<img src="docs/img/logo.png?raw=true" alt="Umbraco Secret Store" width="250" align="right" />

[![NuGet release](https://img.shields.io/nuget/v/Our.Umbraco.SecretStore.svg)](https://www.nuget.org/packages/Our.Umbraco.SecretStore/)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.com/packages/developer-tools/meganav/)

Secret Store allows you to securely manage and store secrets in the Umbraco backoffice.

Values are encrypted and stored in the Umbraco database; with a handy service for access in code, and an admin-only backoffice section to manage them.

## Getting started

This package is supported on Umbraco 8.1+.

### Installation

Secret Store is available from Our Umbraco, NuGet, or as a manual download directly from GitHub.

#### Our Umbraco repository

You can find a downloadable package on the [Our Umbraco](https://our.umbraco.com/packages/developer-tools/secret-store/) site.

#### NuGet package repository

To [install from NuGet](https://www.nuget.org/packages/Our.Umbraco.SecretStore/), run the following command in your instance of Visual Studio.

    PM> Install-Package Our.Umbraco.SecretStore

## Usage

After installation the "Settings" section in the backoffice will show a "Secrets" tree item, where values can be added and removed. Only users with access to the "Settings" section are able to see this option.

For security reasons it is not possible to view secret values in the backoffice, only a list of existing keys and their last updated dates.

To replace the value for an existing key, either:

- Delete the existing key can add it again with the new value
- Create a new key with the same name as the existing key - it will replace the existing value

### Accessing secrets

To retrieve a secret value in code the `ISecretService` must be used, like this:

```
public class ExampleClass
{
    private readonly ISecretService _secretService;

    public ExampleClass(ISecretService secretService)
    {
        _secretService = secretService;
    }

    public void DoSomething()
    {
        var secret = _secretService.GetSecret("SomeSecretKey");
    }
}
```

This process will cause a database lookup, so it is recommended to cache the result or store it within a [Singleton lifetime](https://our.umbraco.com/Documentation/Reference/Using-Ioc/index-v8#registering-dependencies) in the DI container.

### Token Provider

By default Secret Store ships with a `MachineKeyTokenProvider` - using the `MachineKey.Protect` and `MachineKey.Unprotect` methods from `System.Web.Security` to secure values.

An `ITokenProvider` can be implemented to change the logic to be anything desired, such as using a different encryption algorithm.

The default `ITokenProvider` used can be replaced within an [Umbraco `Composer`](https://our.umbraco.com/Documentation/Implementation/Composing/index-v8) like this:

```
public class ExampleComposer : IUserComposer
{
    public void Compose(Composition composition)
    {
        composition.RegisterUnique<ITokenProvider, CustomTokenProvider>();
    }
}
```

## Contribution guidelines

To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the library.

### Who do I talk to?

This project is maintained by [Callum Whyte](https://callumwhyte.com/) and contributors. If you have any questions about the project please get in touch on [Twitter](https://twitter.com/callumbwhyte), or by raising an issue on GitHub.

## Credits

The Secret Store logo uses the [Lock](https://thenounproject.com/term/lock/4729702/) icon from the [Noun Project](https://thenounproject.com) by [Enjang Solehudin](https://thenounproject.com/enjangsolehudin139/), licensed under [CC BY 3.0 US](https://creativecommons.org/licenses/by/3.0/us/).

## License

Copyright &copy; 2022 [Callum Whyte](https://callumwhyte.com/), and other contributors

Licensed under the [MIT License](LICENSE.md).