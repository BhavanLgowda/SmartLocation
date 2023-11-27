using SmartLocationApp.Models;
using SmartLocationApp.Source;

namespace SmartLocationApp.Router
{
  public interface PageRouter
  {
    void goModeSelection(PageRouter router);

    void goModeSetting(PageRouter router);

    void goScanBarocdePage(PageRouter router);

    void goHomePage(PageRouter router);

    void goHomePage(PageRouter router, Datas data);

    void goPhototakenPlaceSetting(PageRouter router);

    void goLocalPhotoSetting(PageRouter router);

    void goPhotoSaleSetting(PageRouter router);

    void goPhotoSaleHomePage(PageRouter router, Datas data);

    void goGalacticTvSetting(PageRouter router);

    void goGalacticTv(PageRouter router, Datas data);

    void goBarcodePrintSettings(PageRouter router);

    void goBarcodePrint(PageRouter router, Datas data);

    void goLocalUploadHomePage(PageRouter router, Datas data);

    void ReSendFailedPodcam(PodcamError podcam);
  }
}
