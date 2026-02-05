import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Search } from "lucide-react";
import { Skeleton } from "@/components/ui/skeleton";
import { DashboardService } from "@/services/api";
import { useQuery } from "@tanstack/react-query";
import { formatDate } from "@/lib/utils";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { useState, useMemo } from "react";

export default function HistoryPage() {
    const [searchTerm, setSearchTerm] = useState("");
    const [statusFilter, setStatusFilter] = useState("all");
    const [dateFilter, setDateFilter] = useState("all");
    const { data: history, isLoading } = useQuery({
        queryKey: ['notificationHistory'],
        queryFn: DashboardService.getNotificationHistory
    });

    const filteredHistory = useMemo(() => {
        if (!history) return [];

        return history.filter(item => {
            // Search Filter
            const searchLower = searchTerm.toLowerCase();
            const matchesSearch =
                item.id.toLowerCase().includes(searchLower) ||
                item.status.toLowerCase().includes(searchLower) ||
                item.notificationId.toLowerCase().includes(searchLower);

            // Status Filter
            const matchesStatus = statusFilter === "all" || item.status === statusFilter;

            // Date Filter
            let matchesDate = true;
            if (dateFilter !== "all") {
                const sentDate = new Date(item.sentDate);
                const today = new Date();
                today.setHours(0, 0, 0, 0);

                if (dateFilter === "today") {
                    const itemDate = new Date(sentDate);
                    itemDate.setHours(0, 0, 0, 0);
                    matchesDate = itemDate.getTime() === today.getTime();
                } else if (dateFilter === "7days") {
                    const sevenDaysAgo = new Date(today);
                    sevenDaysAgo.setDate(today.getDate() - 7);
                    matchesDate = sentDate >= sevenDaysAgo;
                } else if (dateFilter === "30days") {
                    const thirtyDaysAgo = new Date(today);
                    thirtyDaysAgo.setDate(today.getDate() - 30);
                    matchesDate = sentDate >= thirtyDaysAgo;
                }
            }

            return matchesSearch && matchesStatus && matchesDate;
        });
    }, [history, searchTerm, statusFilter, dateFilter]);


    return (
        <div className="space-y-8">
            <div className="space-y-1">
                <h2 className="text-3xl font-bold tracking-tight text-orange-500">
                    Notification History
                </h2>
                <p className="text-muted-foreground">Comprehensive log of all notification delivery attempts.</p>
            </div>

            <Card className="border-border/50 bg-card/50 backdrop-blur-sm shadow-xl">
                <CardHeader>
                    <div className="flex items-center justify-between">
                        <CardTitle>History Log</CardTitle>
                        <div className="flex flex-col sm:flex-row gap-4 items-center w-full sm:w-auto">
                            <Select value={statusFilter} onValueChange={setStatusFilter}>
                                <SelectTrigger className="w-[140px] bg-background/50">
                                    <SelectValue placeholder="Filter by Status" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="all">All Status</SelectItem>
                                    <SelectItem value="Sent">Sent</SelectItem>
                                    <SelectItem value="Delivered">Delivered</SelectItem>
                                    <SelectItem value="Failed">Failed</SelectItem>
                                </SelectContent>
                            </Select>

                            <Select value={dateFilter} onValueChange={setDateFilter}>
                                <SelectTrigger className="w-[140px] bg-background/50">
                                    <SelectValue placeholder="Date Range" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="all">All Time</SelectItem>
                                    <SelectItem value="today">Today</SelectItem>
                                    <SelectItem value="7days">Last 7 Days</SelectItem>
                                    <SelectItem value="30days">Last 30 Days</SelectItem>
                                </SelectContent>
                            </Select>

                            <div className="relative w-full sm:w-64">
                                <Search className="absolute left-2 top-2.5 h-4 w-4 text-muted-foreground" />
                                <Input
                                    placeholder="Search history..."
                                    className="pl-8 bg-background/50"
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                />
                            </div>
                        </div>
                    </div>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow className="hover:bg-transparent">
                                <TableHead>History ID</TableHead>
                                <TableHead>Notification ID</TableHead>
                                <TableHead>Status</TableHead>
                                <TableHead className="text-right">Sent Date</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {isLoading ? (
                                Array.from({ length: 5 }).map((_, i) => (
                                    <TableRow key={i}>
                                        <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                                        <TableCell><Skeleton className="h-4 w-24" /></TableCell>
                                        <TableCell><Skeleton className="h-6 w-16 rounded-full" /></TableCell>
                                        <TableCell className="text-right"><Skeleton className="h-4 w-32 ml-auto" /></TableCell>
                                    </TableRow>
                                ))
                            ) : (
                                filteredHistory?.map((item) => (
                                    <TableRow key={item.id} className="group hover:bg-muted/50 transition-colors">
                                        <TableCell className="font-medium font-mono text-xs text-muted-foreground group-hover:text-foreground">{item.id}</TableCell>
                                        <TableCell className="font-mono text-xs">{item.notificationId}</TableCell>
                                        <TableCell>
                                            <Badge
                                                variant="secondary"
                                                className={`
                                                    ${item.status === 'Sent' && 'bg-blue-500/15 text-blue-600'}
                                                    ${item.status === 'Delivered' && 'bg-emerald-500/15 text-emerald-600'}
                                                    ${item.status === 'Failed' && 'bg-red-500/15 text-red-600'}
                                                `}
                                            >
                                                {item.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell className="text-right text-muted-foreground">{formatDate(item.sentDate)}</TableCell>
                                    </TableRow>
                                ))
                            )}
                            {!isLoading && filteredHistory?.length === 0 && (
                                <TableRow>
                                    <TableCell colSpan={6} className="h-24 text-center">
                                        No history found.
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>
        </div>
    );
}
