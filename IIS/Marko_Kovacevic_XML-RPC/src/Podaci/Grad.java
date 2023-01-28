package Podaci;

import java.math.BigDecimal;

public class Grad {

    private String naziv;
    private BigDecimal temperatura;

    public Grad(String naziv, BigDecimal temperatura) {
        this.naziv = naziv;
        this.temperatura = temperatura;
    }

    public String getNaziv() {
        return naziv;
    }

    public void setNaziv(String naziv) {
        this.naziv = naziv;
    }

    public BigDecimal getTemperatura() {
        return temperatura;
    }

    public void setTemperatura(BigDecimal temperatura) {
        this.temperatura = temperatura;
    }
}
