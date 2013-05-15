using Deje.Core.Repository;
using Deje.Core.Utils;

namespace Deje.Core.Manager
{
    public class DobavljaciManager
    {
        private readonly IDobavljaciRepository m_DobavljaciRepository;

        public DobavljaciManager(IDobavljaciRepository dobavljaciRepository)
        {
            m_DobavljaciRepository = dobavljaciRepository;
        }

        public void SacuvajLokacijuDobavljaca(int idDobavlajca, double latitude, double longitude, int zoom)
        {
            var dobavljac = m_DobavljaciRepository.VratiPoId(idDobavlajca);
            dobavljac.GpsLokacija = GeoUtils.CreatePoint(latitude, longitude);
            dobavljac.Zoom = zoom;
            m_DobavljaciRepository.Save(dobavljac);
        }
    }
}