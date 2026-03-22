using CommunityToolkit.Mvvm.Messaging;

using XsltEditor.Models.Messages;
using XsltEditor.Services.Abstractions;
using XsltEditor.Transform;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services;

internal class TransformService : ITransformService
{
	private EngineType _currentType;

	public TransformService(ISettingsService settingsService, IMessenger messenger)
	{
		_currentType = settingsService.Settings.EngineType;

		messenger.Register<EngineChangedMessage>(this, (_, m) => _currentType = m.EngineType);
	}

	public void SetEngine(EngineType engineType)
	{
		_currentType = engineType;
	}

	public Task WarmupAsync()
	{
		return Task.Run(XsltEngineFactory.WarmupAll);
	}

	public Task<string> TransformAsync(string xml, string xsl, string? baseUri = null)
	{
		return XsltEngineFactory.Get(_currentType).TransformAsync(xml, xsl, baseUri);
	}
}