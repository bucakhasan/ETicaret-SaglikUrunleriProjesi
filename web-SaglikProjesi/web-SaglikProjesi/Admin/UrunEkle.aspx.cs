using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace web_SaglikProjesi.Admin
{
    public partial class UrunEkle : System.Web.UI.Page
    {
        DataModel.SaglikUrunleriEntities ent = new DataModel.SaglikUrunleriEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Admin"] != null)
                {
                    Panel pnl = (Panel)this.Master.FindControl("pnlMenu");
                    pnl.Visible = true;
                    KategorileriGetir();
                    AltKategorilerGetirByKategoriNo(Convert.ToInt32(ddlKategoriler.SelectedValue));
                    UrunleriGetirByKategoriAndAltKategori(Convert.ToInt32(ddlKategoriler.SelectedValue), Convert.ToInt32(ddlAltKategoriler.SelectedValue));
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
        private void KategorileriGetir()
        {
            var categories = (from c in ent.Kategoriler
                              where c.silindi == false
                              select c).ToList();
            ddlKategoriler.DataTextField = "kategoriad";
            ddlKategoriler.DataValueField = "id";
            ddlKategoriler.DataSource = categories;
            ddlKategoriler.DataBind();
        }
        protected void ddlKategoriler_SelectedIndexChanged(object sender, EventArgs e)
        {
            AltKategorilerGetirByKategoriNo(Convert.ToInt32(ddlKategoriler.SelectedValue));
            UrunleriGetirByKategoriAndAltKategori(Convert.ToInt32(ddlKategoriler.SelectedValue), Convert.ToInt32(ddlAltKategoriler.SelectedValue));
        }
        private void AltKategorilerGetirByKategoriNo(int KategoriNo)
        {
            var subcategories = (from subc in ent.AltKategoriler
                                 where subc.kategorino == KategoriNo && subc.silindi == false
                                 select subc).ToList();
            ddlAltKategoriler.DataTextField = "altkategoriad";
            ddlAltKategoriler.DataValueField = "id";
            ddlAltKategoriler.DataSource = subcategories;
            ddlAltKategoriler.DataBind();
        }
        protected void ddlAltKategoriler_SelectedIndexChanged(object sender, EventArgs e)
        {
            UrunleriGetirByKategoriAndAltKategori(Convert.ToInt32(ddlKategoriler.SelectedValue), Convert.ToInt32(ddlAltKategoriler.SelectedValue));
        }
        private void UrunleriGetirByKategoriAndAltKategori(int KategoriNo, int AltKategoriNo)
        {
            var Products = (from p in ent.Urunler
                            where p.urunkategorino == KategoriNo && p.urunaltkategorino == AltKategoriNo && p.silindi == false
                            select p).ToList();
            gvUrunler.DataSource = Products;
            gvUrunler.DataBind();
        }
        protected void gvUrunler_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void lbEkle_Click(object sender, EventArgs e)
        {
            pnlEkle.Visible = true;
            Temizle();
        }
        protected void btnKaydet_Click(object sender, EventArgs e)
        {
            if(txtUrunKodu.Text.Trim() != "" && txtUrunAdi.Text.Trim() != "")
            {
                if(fuKucukResim.HasFile)
                {
                    if(fuKucukResim.PostedFile.ContentType == "image/jpeg" ) //jpeg uzantılı dosyaları kaydetmek için
                    {
                        //if(fuKucukResim.PostedFile.ContentLength <= 1024000) //1 MB dan küçük dosyaları kabul etmek için
                        //{
                        //    //string isim = Guid.NewGuid().ToString(); //Farklı, random bir sayı üretir.


                        //}
                        fuKucukResim.SaveAs(Server.MapPath("/Content/urunimages/" + fuKucukResim.FileName));
                    }
                }
                if (fuBuyukResim.HasFile)
                {
                    if (fuBuyukResim.PostedFile.ContentType == "image/jpeg") //jpeg uzantılı dosyaları kaydetmek için
                    {
                        //if(fuKucukResim.PostedFile.ContentLength <= 1024000) //1 MB dan küçük dosyaları kabul etmek için
                        //{
                        //    //string isim = Guid.NewGuid().ToString(); //Farklı, random bir sayı üretir.


                        //}
                        fuBuyukResim.SaveAs(Server.MapPath("/Content/urunimages/buyuk/" + fuBuyukResim.FileName));
                    }
                }
                DataModel.Urunler u = new DataModel.Urunler();
                u.urunkodu = txtUrunKodu.Text;
                u.urunad = txtUrunAdi.Text;
                u.urunbilgisi = txtUrunBilgisi.Text;
                u.miktar = Convert.ToInt32(txtMiktar.Text);
                u.urunfiyat = Convert.ToDecimal(txtFiyat.Text);
                u.urunkategorino = Convert.ToInt32(ddlKategoriler.SelectedValue);
                u.urunaltkategorino = Convert.ToInt32(ddlAltKategoriler.SelectedValue);
                u.urunresimyolu1 = "Content/urunimages/" + fuKucukResim.FileName;
                u.urunresimyolu2 = "Content/urunimages/buyuk/" + fuBuyukResim.FileName;
                ent.Urunler.Add(u);
                try
                {
                    ent.SaveChanges();
                    Temizle();
                    pnlEkle.Visible = false;
                    UrunleriGetirByKategoriAndAltKategori(Convert.ToInt32(ddlKategoriler.SelectedValue), Convert.ToInt32(ddlAltKategoriler.SelectedValue));
                }
                catch (Exception ex)
                {
                    string hata = ex.Message;
                }
            }
        }
        protected void btnDegistir_Click(object sender, EventArgs e)
        {

        }
        protected void btnSil_Click(object sender, EventArgs e)
        {

        }
        private void Temizle()
        {
            txtUrunKodu.Text = "";
            txtUrunAdi.Text = "";
            txtUrunBilgisi.Text = "";
            txtMiktar.Text = "1";
            txtFiyat.Text = "0";
            fuKucukResim.Controls.Clear();
            fuBuyukResim.Controls.Clear();
            txtUrunKodu.Focus();
        }

    }
}