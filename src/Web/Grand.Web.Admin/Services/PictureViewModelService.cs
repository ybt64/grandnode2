﻿using Grand.Business.Common.Extensions;
using Grand.Business.Common.Interfaces.Localization;
using Grand.Business.Storage.Interfaces;
using Grand.Web.Admin.Extensions;
using Grand.Web.Admin.Interfaces;
using Grand.Web.Admin.Models.Common;

namespace Grand.Web.Admin.Services
{
    public class PictureViewModelService : IPictureViewModelService
    {
        private readonly IPictureService _pictureService;
        private readonly ILanguageService _languageService;

        public PictureViewModelService(IPictureService pictureService, ILanguageService languageService)
        {
            _pictureService = pictureService;
            _languageService = languageService;
        }

        public virtual async Task<PictureModel> PreparePictureModel(string pictureId, string objectId)
        {
            if (string.IsNullOrEmpty(pictureId))
                throw new ArgumentNullException(nameof(pictureId));

            if (string.IsNullOrEmpty(objectId))
                throw new ArgumentNullException(nameof(objectId));

            var picture = await _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            var model = new PictureModel {
                Id = picture.Id,
                ObjectId = objectId,
                PictureUrl = picture != null ? await _pictureService.GetPictureUrl(picture) : null,
                AltAttribute = picture?.AltAttribute,
                TitleAttribute = picture?.TitleAttribute,
            };

            foreach (var language in await _languageService.GetAllLanguages(true))
            {
                var locale = new PictureModel.PictureLocalizedModel();
                locale.LanguageId = language.Id;
                locale.AltAttribute = picture.GetTranslation(x => x.AltAttribute, language.Id, false);
                locale.TitleAttribute = picture.GetTranslation(x => x.TitleAttribute, language.Id, false);
                model.Locales.Add(locale);
            }

            return model;
        }

        public virtual async Task UpdatePicture(PictureModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var picture = await _pictureService.GetPictureById(model.Id);
            if (picture == null)
                throw new ArgumentNullException(nameof(picture));

            //Update picture fields
            await _pictureService.UpdatePictureField(picture, x => x.AltAttribute, model.AltAttribute);
            await _pictureService.UpdatePictureField(picture, x => x.TitleAttribute, model.TitleAttribute);
            await _pictureService.UpdatePictureField(picture, x => x.Locales, model.Locales.ToTranslationProperty());
        }
    }
}
