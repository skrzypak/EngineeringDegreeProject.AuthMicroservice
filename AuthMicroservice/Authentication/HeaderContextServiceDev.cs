using System.Collections.Generic;
using Authentication.Json;
using Microsoft.AspNetCore.Http;

namespace Authentication
{
    public class HeaderContextServiceDev : HeaderContextService
    {

        public HeaderContextServiceDev(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public override List<Claim_e2ud_item> GetClaim_e2ud()
        {
            if (Header is not null)
            {
                return new List<Claim_e2ud_item>()
                {
                    new Claim_e2ud_item()
                    {
                        epsId = -1,
                        eudId = -11
                    },
                    new Claim_e2ud_item()
                    {
                        epsId = -2,
                        eudId = -22
                    },
                    new Claim_e2ud_item()
                    {
                        epsId = -3,
                        eudId = -33
                    },
                    new Claim_e2ud_item()
                    {
                        epsId = -4,
                        eudId = -44
                    },
                    new Claim_e2ud_item()
                    {
                        epsId = -5,
                        eudId = -55
                    }
                };
            }
            else
            {
                return null;
            }
        }
    }
}
