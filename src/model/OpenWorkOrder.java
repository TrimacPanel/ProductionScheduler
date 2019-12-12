package model;

import java.util.Date;

public class OpenWorkOrder extends WorkOrder{

    private Date schDate;
    private String prodLine;
    private String workShift;
    private int prioritySeq;

    OpenWorkOrder(Date scheduledDate, String assignedProductionLine, String assignedWorkShift, int prioritySequence,
                  Date dueDate, int workOrderNumber, String macolaReference, int quantity, String finishedSKU, String itemDescription,
                  String substrateSKU, String filmSKU, int unitSize, String salesOrder){
        super(dueDate, workOrderNumber, macolaReference, quantity, finishedSKU, itemDescription, substrateSKU, filmSKU,
                unitSize, salesOrder);

        schDate = scheduledDate;
        prodLine = assignedProductionLine;
        workShift = assignedWorkShift;
        prioritySeq = prioritySequence;
    }

    public Date getSchDate() {
        return schDate;
    }

    public void setSchDate(Date schDate) {
        this.schDate = schDate;
    }

    public String getProdLine() {
        return prodLine;
    }

    public void setProdLine(String prodLine) {
        this.prodLine = prodLine;
    }

    public String getWorkShift() {
        return workShift;
    }

    public void setWorkShift(String workShift) {
        this.workShift = workShift;
    }

    public int getPrioritySeq() {
        return prioritySeq;
    }

    public void setPrioritySeq(int prioritySeq) {
        this.prioritySeq = prioritySeq;
    }
}
